// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IAppRepository
    {
        Task<App> AddAsync(App app);

        Task AddEnvironmentClusterProjectAppsAsync(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps);

        Task<App> GetAsync(int Id);

        Task<App> GetByIdenityAsync(string identity);

        Task<List<App>> GetByAppTypesAsync(List<AppTypes> appTypes);

        Task<List<App>> GetListAsync();

        Task<List<App>> GetListByProjectIdAsync(IEnumerable<int> projectIds);

        Task<List<(int AppId, int ProjectId, string ClusterName, string EnvironmentName, string EnvColor, EnvironmentCluster)>> GetEnvironmentAndClusterNamesByAppIds(IEnumerable<int> appIds);

        Task<List<EnvironmentClusterProjectApp>> GetEnvironmentClusterProjectAppsAsync(int environmentClusterProjectId, int appId);

        Task<List<EnvironmentClusterProjectApp>> GetEnvironmentClusterProjectAppsByAppId(int appId);

        Task<List<(int ProjectId, App App)>> GetAppByEnvNameAndProjectIdsAsync(string envName, IEnumerable<int> projectIds);

        Task UpdateAsync(App app);

        Task RemoveAsync(int Id);

        Task RemoveEnvironmentClusterProjectApps(int appId, IEnumerable<int> envClusterProjectIds);

        Task RemoveEnvironmentClusterProjectApps(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps);

        Task IsExistedApp(string name, string identity, List<int> environmentClusterProjectIds, params int[] excludeAppIds);
    }
}
