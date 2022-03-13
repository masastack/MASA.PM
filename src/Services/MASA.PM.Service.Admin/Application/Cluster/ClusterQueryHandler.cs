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
            query.Result = await _clusterRepository.GetAsync(query.ClusterId);
        }

        [EventHandler]
        public async Task ClusterListQueryHandle(ClustersQuery query)
        {
            query.Result = await _clusterRepository.GetListAsync();
        }

        [EventHandler]
        public async Task EnvironmentClustersQueryHandle(EnvironmentClustersQuery query)
        {
            if (query.EnvId.HasValue)
            {
                query.Result = await _clusterRepository.GetListByEnvIdAsync(query.EnvId.Value);
            }
            else
            {
                query.EnvironmentClusters = await _clusterRepository.GetEnvironmentClusters();
            }
        }
    }
}
