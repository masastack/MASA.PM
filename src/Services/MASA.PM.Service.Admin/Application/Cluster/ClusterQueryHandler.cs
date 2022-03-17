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
        public async Task ClusterQueryHandle(ClusterQuery query)
        {
            var cluster = await _clusterRepository.GetAsync(query.ClusterId);
            var envclusters = await _clusterRepository.GetEnvironmentClustersByClusterIdAsync(query.ClusterId);

            query.Result = new ClusterViewModel
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
        public async Task ClusterListQueryHandle(ClustersQuery query)
        {
            var resule = await (await _clusterRepository.GetListAsync()).ToListAsync();
            query.Result = resule.Select(cluster => new ClustersViewModel
            {
                Id = cluster.Id,
                Name = cluster.Name,
            }).ToList();
        }

        [EventHandler]
        public async Task EnvironmentClustersQueryHandle(EnvironmentClustersQuery query)
        {
            if (query.EnvId.HasValue)
            {
                var envCluster = await _clusterRepository.GetEnvironmentClustersByEnvIdAsync(query.EnvId.Value);

                var cluster = await (await _clusterRepository.GetListAsync())
                    .Where(cluster => envCluster.Select(ec => ec.ClusterId).Contains(cluster.Id))
                    .Select(cluster => new Infrastructure.Entities.Cluster
                    {
                        Id = cluster.Id,
                        Name = cluster.Name
                    }).ToListAsync();

                var result = from ec in envCluster
                             join c in cluster on ec.ClusterId equals c.Id
                             select new ClustersViewModel
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
                    string ClusterName)>
                    result = await _clusterRepository.GetEnvironmentClusters();

                query.EnvironmentClusters = result.Select(envClusterGroup => new EnvironmentClusterViewModel
                {
                    Id = envClusterGroup.EnvClusterId,
                    EnvironmentName = envClusterGroup.EnvName,
                    ClusterName = envClusterGroup.ClusterName
                }).ToList();
            }
        }
    }
}
