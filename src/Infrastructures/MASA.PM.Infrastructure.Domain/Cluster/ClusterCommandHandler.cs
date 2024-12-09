// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Cluster;

internal class ClusterCommandHandler
{
    private readonly IClusterRepository _clusterRepository;

    public ClusterCommandHandler(IClusterRepository clusterRepository)
    {
        _clusterRepository = clusterRepository;
    }

    [EventHandler]
    public async Task AddEnvironmentWithClustersAsync(AddClusterCommand command)
    {
        var addClusterEntity = new Shared.Entities.Cluster
        {
            Name = command.ClustersWhitEnvironmentModel.Name,
            Description = command.ClustersWhitEnvironmentModel.Description
        };
        var newCluster = await _clusterRepository.AddAsync(addClusterEntity);

        var addEnvironmentClusters = new List<EnvironmentCluster>();
        command.ClustersWhitEnvironmentModel.EnvironmentIds.ForEach(environmentId =>
        {
            addEnvironmentClusters.Add(new EnvironmentCluster
            {
                ClusterId = newCluster.Id,
                EnvironmentId = environmentId
            });
        });
        await _clusterRepository.AddEnvironmentClusters(addEnvironmentClusters);

        command.Result = new ClusterDto { Id = newCluster.Id, Name = newCluster.Name };
    }

    [EventHandler]
    public async Task UpdateClusterAsync(UpdateClusterCommand command)
    {
        var updateClusterModel = command.UpdateClusterModel;
        var cluster = await _clusterRepository.GetAsync(updateClusterModel.ClusterId);
        cluster.Name = updateClusterModel.Name;
        cluster.Description = updateClusterModel.Description;
        await _clusterRepository.UpdateAsync(cluster);

        var oldEnvironmentIds = (
                await _clusterRepository.GetEnvironmentClustersByClusterIdAsync(updateClusterModel.ClusterId)
            )
            .Select(environmentCluster => environmentCluster.EnvironmentId)
            .ToList();

        // EnvironmentClusters need to delete
        var deleteEnvironmentIds = oldEnvironmentIds.Except(updateClusterModel.EnvironmentIds);
        if (deleteEnvironmentIds.Any())
        {
            var deleteEnvironmentClusters = await _clusterRepository.GetEnvironmentClustersByClusterIdAndEnvironmentIdsAsync(updateClusterModel.ClusterId, deleteEnvironmentIds);
            await _clusterRepository.RemoveEnvironmentClusters(deleteEnvironmentClusters);
        }

        // EnvironmentClusters need to insert
        var addEnvironmentIds = updateClusterModel.EnvironmentIds.Except(oldEnvironmentIds);
        if (addEnvironmentIds.Any())
        {
            await _clusterRepository.AddEnvironmentClusters(addEnvironmentIds.Select(environmentId => new EnvironmentCluster
            {
                EnvironmentId = environmentId,
                ClusterId = updateClusterModel.ClusterId
            }));
        }
    }

    [EventHandler]
    public async Task RemoveClusterAsync(RemoveClusterCommand command)
    {
        await _clusterRepository.RemoveAsync(command.Id);
    }
}
