// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IClusterRepository
    {
        Task<Cluster> AddAsync(Cluster cluster);

        Task AddEnvironmentClusters(IEnumerable<EnvironmentCluster> environmentClusters);

        Task<List<Cluster>> GetListAsync();

        Task<List<EnvironmentCluster>> GetEnvironmentClustersByEnvIdAsync(int envId);

        Task<List<EnvironmentCluster>> GetEnvironmentClustersByClusterIdAsync(int clusterId);

        Task<List<EnvironmentCluster>> GetEnvironmentClustersByClusterIdAndEnvironmentIdsAsync(int clusterId, IEnumerable<int> environmentIds);

        Task<Cluster> GetAsync(int Id);

        Task<List<(int EnvClusterId, string EnvName, string EnvColor, string ClusterName)>> GetEnvironmentClusters();

        Task UpdateAsync(Cluster cluster);

        Task RemoveAsync(int Id);

        Task RemoveEnvironmentClusters(List<EnvironmentCluster> environmentClusters);

        Task RemoveEnvironmentClusterProjects(List<EnvironmentClusterProject> environmentClusterProjects);
    }
}
