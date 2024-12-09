// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Services;

public class ProjectService : ServiceBase
{
    public ProjectService()
    {
        RouteOptions.DisableAutoMapRoute = true;
        App.MapPost("/api/v1/project", AddAsync);
        App.MapGet("/api/v1/projects", GetListAsync);
        App.MapPost("/api/v1/project/teamProjects", GetListByTeamIds);
        App.MapGet("/api/v1/{environmentClusterId}/project", GetListByEnvironmentClusterId);
        App.MapGet("/api/v1/project/{Id}", GetAsync);
        App.MapGet("/api/v1/project/projectType", GetProjectTypes);
        App.MapGet("/api/v1/project/isExistProjectInCluster/{ClusterId}", IsExistProjectInCluster);
        App.MapPut("/api/v1/project", UpdateAsync);
        App.MapDelete("/api/v1/project", RemoveAsync);
    }

    public async Task AddAsync(IEventBus eventBus, AddProjectDto model)
    {
        var command = new AddProjectCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task<List<ProjectDto>> GetListByEnvironmentClusterId(IEventBus eventBus, int environmentClusterId)
    {
        var query = new ProjectsQuery(environmentClusterId, null);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<ProjectDto>> GetListByTeamIds(IEventBus eventBus, [FromBody] List<Guid> teamIds)
    {
        var query = new ProjectsQuery(null, teamIds);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<bool> IsExistProjectInCluster(IEventBus eventBus, int clusterId)
    {
        var query = new ProjectByClusterIdQuery(clusterId);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<ProjectDto>> GetListAsync(IEventBus eventBus)
    {
        var query = new ProjectListQuery();
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<ProjectDetailDto> GetAsync(IEventBus eventBus, int Id)
    {
        var query = new ProjectQuery
        {
            ProjectId = Id
        };
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<ProjectTypesDto>> GetProjectTypes(IEventBus eventBus)
    {
        var query = new ProjectTypesQuery();

        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task UpdateAsync(IEventBus eventBus, UpdateProjectDto model)
    {
        var command = new UpdateProjectCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task RemoveAsync(IEventBus eventBus, [FromBody] int Id)
    {
        var command = new RemoveProjectCommand(Id);
        await eventBus.PublishAsync(command);
    }
}
