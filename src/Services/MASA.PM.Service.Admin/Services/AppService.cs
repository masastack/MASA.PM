﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Services;

internal class AppService : ServiceBase
{
    public AppService()
    {
        RouteOptions.DisableAutoMapRoute = true;
        App.MapPost("/api/v1/app", AddAsync).RequireAuthorization();
        App.MapGet("/api/v1/app", GetListAsync).RequireAuthorization();
        App.MapPost("/api/v1/projects/app", GetListByProjectIdsAsync).RequireAuthorization();
        App.MapGet("/api/v1/app/{Id}", GetAsync).RequireAuthorization();
        App.MapGet("/api/v1/appWhitEnvCluster/{Id}", GetWithEnvironmentClusterAsync).RequireAuthorization();
        App.MapPut("/api/v1/app", UpdateAsync).RequireAuthorization();
        App.MapDelete("/api/v1/app/{id}", RemoveAsync).RequireAuthorization();
    }

    public async Task AddAsync(IEventBus eventBus, AddAppDto model)
    {
        var command = new AddAppCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task<List<AppDto>> GetListAsync(IEventBus eventBus)
    {
        var query = new AppsQuery(new List<int>());
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<AppDto>> GetListByProjectIdsAsync(IEventBus eventBus, [FromBody] List<int> projectIds)
    {
        var query = new AppsQuery(projectIds);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<AppDto> GetWithEnvironmentClusterAsync(IEventBus eventBus, int Id)
    {
        var query = new AppQuery(true, Id);

        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<AppDto> GetAsync(IEventBus eventBus, int Id)
    {
        var query = new AppQuery(false, Id);

        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task UpdateAsync(IEventBus eventBus, UpdateAppDto model)
    {
        var command = new UpdateAppCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task RemoveAsync(IEventBus eventBus, int id)
    {
        var command = new RemoveAppCommand(id);
        await eventBus.PublishAsync(command);
    }
}
