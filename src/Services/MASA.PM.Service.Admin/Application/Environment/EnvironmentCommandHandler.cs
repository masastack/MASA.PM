// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentCommandHandler
    {
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IClusterRepository _clusterRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAppRepository _appRepository;

        public EnvironmentCommandHandler(IEnvironmentRepository environmentRepository,
            IClusterRepository clusterRepository,
            IProjectRepository projectRepository,
            IAppRepository appRepository)
        {
            _environmentRepository = environmentRepository;
            _clusterRepository = clusterRepository;
            _projectRepository = projectRepository;
            _appRepository = appRepository;
        }

        [EventHandler]
        public async Task InitAsync(InitCommand command)
        {
            //environment
            var envs = command.InitModel.Environments.Select(e => new Infrastructure.Entities.Environment
            {
                Name = e.Name,
                Description = e.Description,
                Color = e.Color
            });
            var envEntitis = new List<Infrastructure.Entities.Environment>();
            foreach (var env in envs)
            {
                var newEnv = await _environmentRepository.AddAsync(env);
                envEntitis.Add(newEnv);
            }

            //cluster
            var cluster = await _clusterRepository.AddAsync(new Infrastructure.Entities.Cluster
            {
                Name = command.InitModel.ClusterName,
                Description = command.InitModel.ClusterDescription
            });
            var envClusters = envEntitis.Select(env => new EnvironmentCluster
            {
                EnvironmentId = env.Id,
                ClusterId = cluster.Id,
            });
            var envClusetr = new List<EnvironmentCluster>();
            foreach (var envCluster in envClusters)
            {
                var newEnvCluster = await _environmentRepository.AddEnvironmentClusterAsync(envCluster);
                envClusetr.Add(newEnvCluster);
            }

            //project
            var projects = SeedData.ProjectApps;
            var projectIds = new List<int>();
            var envClusterProject = new List<EnvironmentClusterProject>();
            var appGroups = new List<(int ProjectId, string ProjectDescription, int AppId, string Description)>();
            var envClusterProjectApps = new List<EnvironmentClusterProjectApp>();
            foreach (var project in projects)
            {
                var newProject = await _projectRepository.AddAsync(project.Adapt<Infrastructure.Entities.Project>());
                projectIds.Add(newProject.Id);

                foreach (var app in project.Apps)
                {
                    var newApp = await _appRepository.AddAsync(app.Adapt<Infrastructure.Entities.App>());
                    appGroups.Add((newProject.Id, newProject.Description, newApp.Id, newApp.Description));
                }

                foreach (var envCluster in envClusetr)
                {
                    var newEnvironmentClusterProject = await _projectRepository.AddEnvironmentClusterProjectAsync(new EnvironmentClusterProject
                    {
                        EnvironmentClusterId = envCluster.Id,
                        ProjectId = newProject.Id
                    });

                    foreach (var app in appGroups)
                    {
                        if (app.ProjectId == newProject.Id)
                        {
                            var envName = envEntitis.Find(env => env.Id == envCluster.EnvironmentId)?.Name.ToLower() ?? "develop";
                            envClusterProjectApps.Add(new EnvironmentClusterProjectApp
                            {
                                EnvironmentClusterProjectId = newEnvironmentClusterProject.Id,
                                AppId = app.AppId,
                                AppURL = $"https://{app.ProjectDescription}-{envName}.masastack.com"
                            });
                        }
                    }
                }
            }

            await _appRepository.AddEnvironmentClusterProjectAppsAsync(envClusterProjectApps);
        }

        [EventHandler]
        public async Task AddEnvironmentWithClustersAsync(AddEnvironmentCommand command)
        {
            var addEnvEntity = new Infrastructure.Entities.Environment
            {
                Name = command.EnvironmentWhitClusterModel.Name,
                Color = command.EnvironmentWhitClusterModel.Color,
                Description = command.EnvironmentWhitClusterModel.Description
            };
            var newEnv = await _environmentRepository.AddAsync(addEnvEntity);

            var addEnvironmentClusters = new List<EnvironmentCluster>();
            command.EnvironmentWhitClusterModel.ClusterIds.ForEach(clusterId =>
            {
                addEnvironmentClusters.Add(new EnvironmentCluster
                {
                    ClusterId = clusterId,
                    EnvironmentId = newEnv.Id
                });
            });
            await _environmentRepository.AddEnvironmentClustersAsync(addEnvironmentClusters);

            command.Result = new EnvironmentDto { Id = newEnv.Id, Name = newEnv.Name, Color = newEnv.Color };
        }

        [EventHandler]
        public async Task UpdateEnvironmentAsync(UpdateEnvironmentCommand command)
        {
            var envModel = command.EnvironmentModel;
            await _environmentRepository.UpdateAsync(envModel);
        }

        [EventHandler]
        public async Task RemoveEnvironmentAsync(RemoveEnvironmentCommand command)
        {
            await _environmentRepository.RemoveAsync(command.EnvironmentId);
        }
    }
}
