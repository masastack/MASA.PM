using MASA.PM.Service.Admin.Infrastructure.Entities;

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IAppRepository
    {
        Task<App> AddAsync(App app);

        Task AddEnvironmentClusterProjectAppsAsync(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps);

        Task<App> GetAsync(int Id);

        Task<List<App>> GetListByProjectIdAsync(int projectId);

        Task<List<AppEnvironmentClusterViewModel>> GetEnvironmentAndClusterNamesByAppIds(IEnumerable<int> appIds);

        Task<List<App>> GetListByEnvironmentClusterProjectIdAsync(int environmentClusterProjectId);

        Task UpdateAsync(App app);

        Task DeleteAsync(int Id);

        Task DeleteEnvironmentClusterProjectAppsByAppIdAsync(int Id);
    }
}
