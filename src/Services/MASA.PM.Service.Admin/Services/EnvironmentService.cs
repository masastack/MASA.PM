﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Services;

internal class EnvironmentService : ServiceBase
{
    public EnvironmentService()
    {
        RouteOptions.DisableAutoMapRoute = true;
        App.MapPost("/api/v1/env/init", InitAsync).RequireAuthorization();
        App.MapPost("/api/v1/env", AddAsync).RequireAuthorization();
        App.MapGet("api/v1/env", GetList);
        App.MapGet("api/v1/env/{Id}", GetAsync).RequireAuthorization();
        App.MapPut("/api/v1/env", UpdateAsync).RequireAuthorization();
        App.MapDelete("/api/v1/env", RemoveAsync).RequireAuthorization();
    }

    public async Task InitAsync(IEventBus eventBus, InitDto model)
    {
        var command = new InitCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task<EnvironmentDto> AddAsync(IEventBus eventBus, AddEnvironmentWhitClustersDto model)
    {
        var command = new AddEnvironmentCommand(model);
        await eventBus.PublishAsync(command);

        return command.Result;
    }

    public async Task<List<EnvironmentDto>> GetList(IEventBus eventBus)
    {
        var query = new EnvironmentsQuery();
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<EnvironmentDetailDto> GetAsync(IEventBus eventBus, int Id)
    {
        var query = new EnvironmentQuery
        {
            EnvironmentId = Id
        };
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task UpdateAsync(IEventBus eventBus, UpdateEnvironmentDto model)
    {
        var command = new UpdateEnvironmentCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task RemoveAsync(IEventBus eventBus, [FromBody] int Id)
    {
        var command = new RemoveEnvironmentCommand(Id);
        await eventBus.PublishAsync(command);
    }
}
