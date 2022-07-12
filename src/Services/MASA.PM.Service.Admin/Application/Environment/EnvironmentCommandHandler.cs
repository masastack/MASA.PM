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
                Color = e.Color,
                Creator = Guid.NewGuid(),
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                Modifier = Guid.NewGuid(),
                IsDeleted = false
            });
            var envIds = new List<int>();
            foreach (var env in envs)
            {
                var newEnv = await _environmentRepository.AddAsync(env);
                envIds.Add(newEnv.Id);
            }

            //cluster
            var cluster = await _clusterRepository.AddAsync(new Infrastructure.Entities.Cluster
            {
                Name = command.InitModel.ClusterName,
                Description = command.InitModel.ClusterDescription
            });
            var envClusters = envIds.Select(envId => new EnvironmentCluster
            {
                EnvironmentId = envId,
                ClusterId = cluster.Id,
            });
            var envClusetrIds = new List<int>();
            foreach (var envCluster in envClusters)
            {
                var newEnvCluster = await _environmentRepository.AddEnvironmentClusterAsync(envCluster);
                envClusetrIds.Add(newEnvCluster.Id);
            }

            //project
            var projects = SeedData.ProjectApps;
            var projectIds = new List<int>();
            var envClusterProject = new List<EnvironmentClusterProject>();
            var appGroups = new List<(int ProjectId, int AppId)>();
            var envClusterProjectApps = new List<EnvironmentClusterProjectApp>();
            foreach (var project in projects)
            {
                var newProject = await _projectRepository.AddAsync(project.Adapt<Infrastructure.Entities.Project>());
                projectIds.Add(newProject.Id);

                foreach (var app in project.Apps)
                {
                    var newApp = await _appRepository.AddAsync(app.Adapt<Infrastructure.Entities.App>());
                    appGroups.Add((newProject.Id, newApp.Id));
                }

                foreach (var envClusterId in envClusetrIds)
                {
                    var newEnvironmentClusterProject = await _projectRepository.AddEnvironmentClusterProjectAsync(new EnvironmentClusterProject
                    {
                        EnvironmentClusterId = envClusterId,
                        ProjectId = newProject.Id
                    });

                    foreach (var appId in appGroups)
                    {
                        if (appId.ProjectId == newProject.Id)
                        {
                            envClusterProjectApps.Add(new EnvironmentClusterProjectApp
                            {
                                EnvironmentClusterProjectId = newEnvironmentClusterProject.Id,
                                AppId = appId.AppId,
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
