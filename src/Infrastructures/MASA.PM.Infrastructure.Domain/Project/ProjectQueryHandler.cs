// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Project;

public class ProjectQueryHandler
{
    private readonly IProjectRepository _projectRepository;
    private readonly IAppRepository _appRepository;
    private readonly IDccClient _dccClient;
    private readonly string _environment;

    public ProjectQueryHandler(IProjectRepository projectRepository, IAppRepository appRepository, IDccClient dccClient, IMultiEnvironmentUserContext multiEnvironmentUserContext, IMasaStackConfig masaStackConfig)
    {
        _projectRepository = projectRepository;
        _appRepository = appRepository;
        _dccClient = dccClient;
        _environment = multiEnvironmentUserContext.Environment ?? masaStackConfig.Environment;
    }

    [EventHandler]
    public async Task GetProject(ProjectQuery query)
    {
        var projectEntity = await _projectRepository.GetAsync(query.ProjectId);
        var environmentCluster = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(projectEntity.Id);
        var projectTeams = await _projectRepository.GetProjectTeamByProjectId(query.ProjectId);
        query.Result = new ProjectDetailDto
        {
            Id = projectEntity.Id,
            Identity = projectEntity.Identity,
            LabelCode = projectEntity.LabelCode,
            Name = projectEntity.Name,
            Description = projectEntity.Description,
            EnvironmentProjectTeams = projectTeams.GroupBy(c => new { c.EnvironmentName, c.ProjectId }).Select(c => new EnvironmentProjectTeamDto
            {
                EnvironmentName = c.Key.EnvironmentName,
                TeamIds = c.Where(p => p.TeamId != Guid.Empty).Select(p => p.TeamId).ToList()
            }).ToList(),
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
        var projectTeams = await _projectRepository.GetProjectTeamByProjectId(projectEntity.Id);
        query.Result = new ProjectDetailDto
        {
            Id = projectEntity.Id,
            Identity = projectEntity.Identity,
            LabelCode = projectEntity.LabelCode,
            Name = projectEntity.Name,
            Description = projectEntity.Description,
            EnvironmentProjectTeams = projectTeams.GroupBy(p => new { p.EnvironmentName, p.ProjectId }).Select(c => new EnvironmentProjectTeamDto
            {
                EnvironmentName = c.Key.EnvironmentName,
                TeamIds = c.Where(p => p.TeamId != Guid.Empty).Select(p => p.TeamId).ToList(),
            }).ToList(),
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
            var projectTeams = (await _projectRepository.GetProjectTeamByProjectIds(projects.Select(c => c.Id)))?.GroupBy(p => new { p.ProjectId, p.EnvironmentName }).ToList();
            query.Result = projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Identity = project.Identity,
                Name = project.Name,
                EnvironmentProjectTeams = projectTeams?.Where(p => p.Key.ProjectId == project.Id).Select(c => new EnvironmentProjectTeamDto
                {
                    EnvironmentName = c.Key.EnvironmentName,
                    TeamIds = c.Where(p => p.TeamId != Guid.Empty).Select(p => p.TeamId).ToList(),
                }).ToList() ?? new(),
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
            (List<Shared.Entities.Project> projects, List<EnvironmentProjectTeam> projectTeams) = await _projectRepository.GetListByTeamIdsAsync(query.TeamIds, query.Environment ?? _environment);
            var projectTeamGroups = projectTeams?.GroupBy(p => new { p.ProjectId, p.EnvironmentName }).ToList();
            query.Result = projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Identity = project.Identity,
                Name = project.Name,
                EnvironmentProjectTeams = projectTeamGroups?.Where(p => p.Key.ProjectId == project.Id).Select(c => new EnvironmentProjectTeamDto
                {
                    EnvironmentName = c.Key.EnvironmentName,
                    TeamIds = c.Where(p => p.TeamId != Guid.Empty).Select(p => p.TeamId).ToList(),
                }).ToList() ?? new(),
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
        var projectTeams = await _projectRepository.GetProjectTeamByProjectIds(projects.Select(c => c.Id));
        var projectTeamGroups = projectTeams?.GroupBy(p => new { p.ProjectId, p.EnvironmentName }).ToList();
        query.Result = projects.Select(project => new ProjectDto
        {
            Id = project.Id,
            Identity = project.Identity,
            Name = project.Name,
            EnvironmentProjectTeams = projectTeamGroups?.Where(p => p.Key.ProjectId == project.Id).Select(c => new EnvironmentProjectTeamDto
            {
                EnvironmentName = c.Key.EnvironmentName,
                TeamIds = c.Where(p => p.TeamId != Guid.Empty).Select(p => p.TeamId).ToList(),
            }).ToList() ?? new(),
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
        var projectTeams = await _projectRepository.GetProjectTeamByProjectIds(projects.Select(c => c.Id));
        projectTeams = projectTeams.Where(c => c.EnvironmentName.ToLower() == query.EnvName.ToLower()).ToList();
        var projectTeamGroups = projectTeams?.GroupBy(p => p.ProjectId).ToList();
        var projectModels = projects.Select(
            project => new ProjectModel(
                project.Id,
                project.Identity,
                project.Name,
                project.LabelCode,
                projectTeamGroups?.FirstOrDefault(p => project.Id == p.Key)?.Where(p => p.TeamId != Guid.Empty).Select(p => p.TeamId).ToList() ?? [])
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
                        appGroup.EnvironmentClusterProjectApp.AppURL,
                        appGroup.App.ServiceType,
                        appGroup.EnvironmentClusterProjectApp.AppSwaggerURL,
                        appGroup.App.Description));
                }
            });
        });

        query.Result = projectModels;
    }

    [EventHandler]
    public async Task IsExistProjectInCluster(ProjectByClusterIdQuery query)
    {
        query.Result = await _projectRepository.IsExistProjectInCluster(query.ClusterId);
    }
}
