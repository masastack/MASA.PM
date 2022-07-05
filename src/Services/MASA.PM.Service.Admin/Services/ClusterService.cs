// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using MASA.PM.Service.Admin.Application.Cluster.Commands;
using MASA.PM.Service.Admin.Application.Cluster.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MASA.PM.Service.Admin.Services
{
    public class ClusterService : ServiceBase
    {
        public ClusterService(IServiceCollection services) : base(services)
        {
            App.MapPost("/api/v1/cluster", AddAsync);
            App.MapGet("/api/v1/cluster", GetList);
            App.MapGet("/api/v1/{envId}/cluster", GetListByEnvId);
            App.MapGet("/api/v1/cluster/{Id}", GetAsync);
            App.MapGet("/api/v1/envClusters", GetEnvironmentClusters);
            App.MapPut("/api/v1/cluster", UpdateAsync);
            App.MapDelete("/api/v1/cluster", RemoveEnvClusterAsync);
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

        public async Task RemoveEnvClusterAsync(IEventBus eventBus, [FromBody] int envClusterId)
        {
            var command = new DeleteClusterCommand(envClusterId);
            await eventBus.PublishAsync(command);
        }
    }
}
