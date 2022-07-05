// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using MASA.PM.Service.Admin.Application.Project.Queries;

namespace MASA.PM.Service.Admin.Application.Project
{
    public class ProjectQueryHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAppRepository _appRepository;

        public ProjectQueryHandler(IProjectRepository projectRepository, IAppRepository appRepository)
        {
            _projectRepository = projectRepository;
            _appRepository = appRepository;
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
                LabelId = projectEntity.LabelId,
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
            var projectTypes = await _projectRepository.GetProjectTypesAsync();
            if (query.EnvironmentClusterId.HasValue)
            {
                var projects = await _projectRepository.GetListByEnvironmentClusterIdAsync(query.EnvironmentClusterId.Value);
                query.Result = projects.Select(project => new ProjectDto
                {
                    Id = project.Id,
                    Identity = project.Identity,
                    Name = project.Name,
                    LabelId = project.LabelId,
                    LabelName = projectTypes.FirstOrDefault(label => label.Id == project.LabelId)?.Name ?? "",
                    Description = project.Description,
                    Modifier = project.Modifier,
                    ModificationTime = project.ModificationTime,
                })
                .OrderByDescending(project => project.ModificationTime)
                .ToList();
            }
            else if (query.TeamId.HasValue)
            {
                var projects = await _projectRepository.GetListByTeamIdAsync(query.TeamId.Value);
                query.Result = projects.Select(project => new ProjectDto
                {
                    Id = project.Id,
                    Identity = project.Identity,
                    Name = project.Name,
                    LabelId = project.LabelId,
                    LabelName = projectTypes.FirstOrDefault(label => label.Id == project.LabelId)?.Name ?? "",
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
            var projectTypes = await _projectRepository.GetProjectTypesAsync();
            var projects = await _projectRepository.GetListAsync();

            query.Result = projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Identity = project.Identity,
                Name = project.Name,
                LabelId = project.LabelId,
                LabelName = projectTypes.FirstOrDefault(label => label.Id == project.LabelId)?.Name ?? "",
                Description = project.Description,
                Modifier = project.Modifier,
                ModificationTime = project.ModificationTime
            }).ToList();
        }

        [EventHandler]
        public async Task GetProjectTypes(ProjectTypesQuery query)
        {
            var result = await _projectRepository.GetProjectTypesAsync();

            query.Result = result.Select(projectType => new ProjectTypesDto
            {
                Id = projectType.Id,
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
                    project.LabelId,
                    project.TeamId)
                ).ToList();

            projectModels.ForEach(project =>
            {
                apps.ForEach(appGroup =>
                {
                    if (appGroup.ProjectId == project.Id)
                    {
                        project.Apps.Add(new AppModel(appGroup.App.Id, appGroup.App.Name, appGroup.App.Identity, project.Id));
                    }
                });
            });

            query.Result = projectModels;
        }
    }
}
