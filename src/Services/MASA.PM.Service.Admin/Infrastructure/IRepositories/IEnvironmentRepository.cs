namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IEnvironmentRepository
    {
        Task<Entities.Environment> AddAsync(Entities.Environment environment);

        Task AddEnvironmentClustersAsync(List<EnvironmentCluster> environmentClusters);

        Task<List<EnvironmentsViewModel>> GetListAsync();

        Task UpdateAsync(UpdateEnvironmentModel model);

        Task<EnvironmentViewModel> GetAsync(int Id);

        Task DeleteAsync(int Id);
    }
}
