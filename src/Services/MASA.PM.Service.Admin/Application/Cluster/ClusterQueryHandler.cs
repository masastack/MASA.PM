// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using MASA.PM.Service.Admin.Application.Cluster.Queries;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class ClusterQueryHandler
    {
        private readonly IClusterRepository _clusterRepository;

        public ClusterQueryHandler(IClusterRepository clusterRepository)
        {
            _clusterRepository = clusterRepository;
        }

        [EventHandler]
        public async Task GetClusterAsync(ClusterQuery query)
        {
            var cluster = await _clusterRepository.GetAsync(query.ClusterId);
            var envclusters = await _clusterRepository.GetEnvironmentClustersByClusterIdAsync(query.ClusterId);

            query.Result = new ClusterDetailDto
            {
                Id = cluster.Id,
                Name = cluster.Name,
                Description = cluster.Description,
                Creator = cluster.Creator,
                CreationTime = cluster.CreationTime,
                Modifier = cluster.Modifier,
                ModificationTime = cluster.ModificationTime,
                EnvironmentIds = envclusters.Select(envCluster => envCluster.EnvironmentId).ToList()
            };
        }

        [EventHandler]
        public async Task GetClusterListAsync(ClustersQuery query)
        {
            var result = (await _clusterRepository.GetListAsync())
                .OrderByDescending(cluster => cluster.ModificationTime)
                .Select(cluster => new ClusterDto
                {
                    Id = cluster.Id,
                    Name = cluster.Name,
                }).ToList();

            query.Result = result;
        }

        [EventHandler]
        public async Task GetEnvironmentClustersAsync(EnvironmentClustersQuery query)
        {
            if (query.EnvId.HasValue)
            {
                var envCluster = await _clusterRepository.GetEnvironmentClustersByEnvIdAsync(query.EnvId.Value);

                var cluster = (await _clusterRepository.GetListAsync())
                    .Where(cluster => envCluster.Select(ec => ec.ClusterId).Contains(cluster.Id))
                    .OrderByDescending(cluster => cluster.ModificationTime)
                    .Select(cluster => new Infrastructure.Entities.Cluster(cluster.Id, cluster.Name, "")).ToList();

                var result = from c in cluster
                             join ec in envCluster on c.Id equals ec.ClusterId
                             select new ClusterDto
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 EnvironmentClusterId = ec.Id
                             };

                query.Result = result.ToList();
            }
            else
            {
                List<(int EnvClusterId,
                    string EnvName,
                    string EnvColor,
                    string ClusterName)>
                    result = await _clusterRepository.GetEnvironmentClusters();

                query.EnvironmentClusters = result.Select(envClusterGroup => new EnvironmentClusterDto
                {
                    Id = envClusterGroup.EnvClusterId,
                    EnvironmentName = envClusterGroup.EnvName,
                    EnvironmentColor = envClusterGroup.EnvColor,
                    ClusterName = envClusterGroup.ClusterName
                }).ToList();
            }
        }
    }
}
