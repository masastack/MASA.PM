// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Services;

internal class OpenApiService : ServiceBase
{
    public OpenApiService()
    {
        RouteOptions.DisableAutoMapRoute = true;
        App.MapGet("/open-api/project/{identity}", GetProjectByIdentityAsync);
        App.MapGet("/open-api/app/{identity}", GetAppByIdentityAsync);
        App.MapPost("/open-api/app/by-types", GetListByTypesAsync);
        App.MapGet("/open-api/projectwithapps/{envName}", GetListByEnvName);
        App.MapPost("/open-api/project/teamProjects/{environment}", GetListByTeamIds);
    }

    public async Task<ProjectDetailDto> GetProjectByIdentityAsync(IEventBus eventBus, string identity)
    {
        var query = new ProjectByIdentityQuery(identity);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<AppDto> GetAppByIdentityAsync(IEventBus eventBus, string identity)
    {
        var query = new AppByIdentityQuery(identity);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<AppDto>> GetListByTypesAsync(IEventBus eventBus, [FromBody] params AppTypes[] appTypes)
    {
        var query = new AppByTypesQuery(appTypes);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<ProjectModel>> GetListByEnvName(IEventBus eventBus, string envName)
    {
        var query = new ProjectAppsQuery(envName);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<ProjectDto>> GetListByTeamIds(IEventBus eventBus, [FromBody] List<Guid> teamIds, string environment)
    {
        var query = new ProjectsQuery(null, teamIds, environment);
        await eventBus.PublishAsync(query);

        return query.Result;
    }
}
