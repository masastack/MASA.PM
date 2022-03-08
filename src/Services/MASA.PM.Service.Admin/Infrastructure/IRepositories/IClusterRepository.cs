namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IClusterRepository
    {
        Task<Cluster> AddAsync(Cluster cluster);

        Task AddEnvironmentClusters(IEnumerable<EnvironmentCluster> environmentClusters);

        Task<List<ClustersViewModel>> GetListAsync();

        Task<List<ClustersViewModel>> GetListByEnvIdAsync(int envId);

        Task<List<EnvironmentCluster>> GetEnvironmentClustersByClusterIdAsync(int clusterId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByClusterIdAsync(IEnumerable<int> environmentClusterIds);

        Task<List<EnvironmentCluster>> GetEnvironmentClustersByClusterIdAndEnvironmentIdsAsync(int clusterId, IEnumerable<int> environmentIds);

        Task<ClusterViewModel> GetAsync(int Id);

        Task<List<EnvironmentCluster>> GetEnvironmentClustersByIds(IEnumerable<int> environmentClusterIds);

        Task UpdateAsync(Cluster cluster);

        Task DeleteAsync(int Id);

        Task DeleteEnvironmentClusters(List<EnvironmentCluster> environmentClusters);

        Task DeleteEnvironmentClusterProjects(List<EnvironmentClusterProject> environmentClusterProjects);
    }
}
