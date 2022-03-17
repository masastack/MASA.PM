namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IEnvironmentRepository
    {
        Task<List<int>> AddEnvironmentsAndClusterAsync(List<Entities.Environment> environments);

        Task<Entities.Environment> AddAsync(Entities.Environment environment);

        Task AddEnvironmentClustersAsync(IEnumerable<EnvironmentCluster> environmentClusters);

        Task<IQueryable<Entities.Environment>> GetListAsync();

        Task UpdateAsync(UpdateEnvironmentDto model);

        Task<Entities.Environment> GetAsync(int Id);

        Task RemoveAsync(int Id);
    }
}
