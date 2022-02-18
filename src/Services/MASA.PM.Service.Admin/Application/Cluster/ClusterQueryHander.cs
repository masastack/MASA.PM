using MASA.PM.Service.Admin.Application.Cluster.Queries;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class ClusterQueryHander
    {
        private readonly IClusterRepository _clusterRepository;

        public ClusterQueryHander(IClusterRepository clusterRepository)
        {
            _clusterRepository = clusterRepository;
        }

        [EventHandler]
        public async Task ClusterQueryHande(ClusterQuery query)
        {
            query.Result = await _clusterRepository.GetAsync(query.ClusterId);
        }

        [EventHandler]
        public async Task ClusterListQueryHande(ClustersQuery query)
        {
            query.Result = await _clusterRepository.GetListAsync();
        }
    }
}
