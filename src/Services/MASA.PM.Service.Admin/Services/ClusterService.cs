// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Services;

internal class ClusterService : ServiceBase
{
    public ClusterService()
    {
        RouteOptions.DisableAutoMapRoute = true;
        App.MapPost("/api/v1/cluster", AddAsync);
        App.MapGet("/api/v1/cluster", GetList);
        App.MapGet("/api/v1/{envId}/cluster", GetListByEnvId);
        App.MapGet("/api/v1/cluster/{Id}", GetAsync);
        App.MapGet("/api/v1/envClusters", GetEnvironmentClusters);
        App.MapPut("/api/v1/cluster", UpdateAsync);
        App.MapDelete("/api/v1/cluster/{id}", RemoveAsync);
    }

    public async Task<ClusterDto> AddAsync(IEventBus eventBus, AddClusterWhitEnvironmentsDto model)
    {
        var command = new AddClusterCommand(model);
        await eventBus.PublishAsync(command);

        return command.Result;
    }

    public async Task<List<ClusterDto>> GetList(IEventBus eventBus)
    {
        var query = new ClustersQuery();
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<ClusterDto>> GetListByEnvId(IEventBus eventBus, int envId)
    {
        var query = new EnvironmentClustersQuery(envId);
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<ClusterDetailDto> GetAsync(IEventBus eventBus, int Id)
    {
        var query = new ClusterQuery
        {
            ClusterId = Id
        };
        await eventBus.PublishAsync(query);

        return query.Result;
    }

    public async Task<List<EnvironmentClusterDto>> GetEnvironmentClusters(IEventBus eventBus)
    {
        var query = new EnvironmentClustersQuery(null);
        await eventBus.PublishAsync(query);

        return query.EnvironmentClusters;
    }

    public async Task UpdateAsync(IEventBus eventBus, UpdateClusterDto model)
    {
        var command = new UpdateClusterCommand(model);
        await eventBus.PublishAsync(command);
    }

    public async Task RemoveAsync(IEventBus eventBus, int id)
    {
        var command = new RemoveClusterCommand(id);
        await eventBus.PublishAsync(command);
    }
}
