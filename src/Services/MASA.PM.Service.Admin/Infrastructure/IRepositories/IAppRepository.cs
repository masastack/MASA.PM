using MASA.PM.Service.Admin.Infrastructure.Entities;

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IAppRepository
    {
        Task<App> AddAsync(App app);

        Task AddEnvironmentClusterProjectAppsAsync(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps);

        Task<App> GetAsync(int Id);

        Task<List<App>> GetListAsync();

        Task<List<App>> GetListByProjectIdAsync(IEnumerable<int> projectIds);

        Task<List<(int AppId, int ProjectId, string ClusterName, string EnvironmentName, EnvironmentCluster)>> GetEnvironmentAndClusterNamesByAppIds(IEnumerable<int> appIds);

        Task<List<EnvironmentClusterProjectApp>> GetEnvironmentClusterProjectAppsAsync(int environmentClusterProjectId, int appId);

        Task<List<EnvironmentClusterProjectApp>> GetEnvironmentClusterProjectAppsByAppId(int appId);

        Task UpdateAsync(App app);

        Task RemoveAsync(int Id);

        Task RemoveEnvironmentClusterProjectApps(int appId, IEnumerable<int> envClusterProjectIds);

        Task RemoveEnvironmentClusterProjectApps(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps);

        Task IsExistedAppName(string name, List<int> environmentClusterProjectIds, params int[] excludeAppIds);
    }
}
