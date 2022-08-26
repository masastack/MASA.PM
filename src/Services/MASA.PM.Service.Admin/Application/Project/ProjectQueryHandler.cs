// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Project
{
    public class ProjectQueryHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAppRepository _appRepository;
        private readonly IDccClient _dccClient;

        public ProjectQueryHandler(IProjectRepository projectRepository, IAppRepository appRepository, IDccClient dccClient)
        {
            _projectRepository = projectRepository;
            _appRepository = appRepository;
            _dccClient = dccClient;
        }

        [EventHandler]
        public async Task GetProject(ProjectQuery query)
        {
            var projectEntity = await _projectRepository.GetAsync(query.ProjectId);
            var environmentCluster = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(projectEntity.Id);
            query.Result = new ProjectDetailDto
            {
                Id = projectEntity.Id,
                Identity = projectEntity.Identity,
                LabelCode = projectEntity.LabelCode,
                Name = projectEntity.Name,
                Description = projectEntity.Description,
                TeamId = projectEntity.TeamId,
                EnvironmentClusterIds = environmentCluster.Select(envCluster => envCluster.EnvironmentClusterId).ToList(),
                CreationTime = projectEntity.CreationTime,
                Creator = projectEntity.Creator,
                Modifier = projectEntity.Modifier,
                ModificationTime = projectEntity.ModificationTime
            };
        }

        [EventHandler]
        public async Task GetProjectByIdentity(ProjectByIdentityQuery query)
        {
            var projectEntity = await _projectRepository.GetByIdentityAsync(query.identity);
            var environmentCluster = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(projectEntity.Id);
            query.Result = new ProjectDetailDto
            {
                Id = projectEntity.Id,
                Identity = projectEntity.Identity,
                LabelCode = projectEntity.LabelCode,
                Name = projectEntity.Name,
                Description = projectEntity.Description,
                TeamId = projectEntity.TeamId,
                EnvironmentClusterIds = environmentCluster.Select(envCluster => envCluster.EnvironmentClusterId).ToList(),
                CreationTime = projectEntity.CreationTime,
                Creator = projectEntity.Creator,
                Modifier = projectEntity.Modifier,
                ModificationTime = projectEntity.ModificationTime
            };
        }

        [EventHandler]
        public async Task GetProjects(ProjectsQuery query)
        {
            var projectTypes = await _dccClient.LabelService.GetListByTypeCodeAsync("ProjectType");
            if (query.EnvironmentClusterId.HasValue)
            {
                var projects = await _projectRepository.GetListByEnvironmentClusterIdAsync(query.EnvironmentClusterId.Value);
                query.Result = projects.Select(project => new ProjectDto
                {
                    Id = project.Id,
                    Identity = project.Identity,
                    Name = project.Name,
                    TeamId = project.TeamId,
                    LabelCode = project.LabelCode,
                    LabelName = projectTypes.FirstOrDefault(label => label.Code == project.LabelCode)?.Name ?? "",
                    Description = project.Description,
                    Modifier = project.Modifier,
                    ModificationTime = project.ModificationTime,
                })
                .OrderByDescending(project => project.ModificationTime)
                .ToList();
            }
            else if (query.TeamIds != null && query.TeamIds.Any())
            {
                var projects = await _projectRepository.GetListByTeamIdsAsync(query.TeamIds);
                query.Result = projects.Select(project => new ProjectDto
                {
                    Id = project.Id,
                    Identity = project.Identity,
                    Name = project.Name,
                    TeamId = project.TeamId,
                    LabelCode = project.LabelCode,
                    LabelName = projectTypes.FirstOrDefault(label => label.Code == project.LabelCode)?.Name ?? "",
                    Description = project.Description,
                    Modifier = project.Modifier,
                    ModificationTime = project.ModificationTime,
                })
                .OrderByDescending(project => project.ModificationTime)
                .ToList();
            }
            else
            {
                query.Result = new();
            }
        }

        [EventHandler]
        public async Task GetProjectListAsync(ProjectListQuery query)
        {
            var projectTypes = await _dccClient.LabelService.GetListByTypeCodeAsync("ProjectType");
            var projects = await _projectRepository.GetListAsync();

            query.Result = projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Identity = project.Identity,
                Name = project.Name,
                TeamId = project.TeamId,
                LabelCode = project.LabelCode,
                LabelName = projectTypes.FirstOrDefault(label => label.Code == project.LabelCode)?.Name ?? "",
                Description = project.Description,
                Modifier = project.Modifier,
                ModificationTime = project.ModificationTime
            }).ToList();
        }

        [EventHandler]
        public async Task GetProjectTypes(ProjectTypesQuery query)
        {
            var result = await _dccClient.LabelService.GetListByTypeCodeAsync("ProjectType");

            query.Result = result.Select(projectType => new ProjectTypesDto
            {
                Id = projectType.Id,
                Code = projectType.Code,
                Name = projectType.Name
            }).ToList();
        }

        [EventHandler]
        public async Task GetListByEnvNameAsync(ProjectAppsQuery query)
        {
            var projects = await _projectRepository.GetProjectListByEnvIdAsync(query.EnvName);
            var apps = await _appRepository.GetAppByEnvNameAndProjectIdsAsync(query.EnvName, projects.Select(project => project.Id));

            List<ProjectModel> projectModels = projects.Select(
                project => new ProjectModel(
                    project.Id,
                    project.Identity,
                    project.Name,
                    project.LabelCode,
                    project.TeamId)
                ).ToList();

            projectModels.ForEach(project =>
            {
                apps.ForEach(appGroup =>
                {
                    if (appGroup.ProjectId == project.Id)
                    {
                        project.Apps.Add(new AppModel(
                            appGroup.App.Id,
                            appGroup.App.Name,
                            appGroup.App.Identity,
                            project.Id,
                            appGroup.App.Type,
                            appGroup.App.ServiceType,
                            appGroup.App.Description));
                    }
                });
            });

            query.Result = projectModels;
        }
    }
}
