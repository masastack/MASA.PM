// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Repository.Repositories;

internal class AppRepository : IAppRepository
{
    private readonly PmDbContext _dbContext;
    private readonly II18n<DefaultResource> _i18N;
    public AppRepository(PmDbContext dbContext, II18n<DefaultResource> i18N)
    {
        _dbContext = dbContext;
        _i18N = i18N;
    }

    public async Task<App> AddAsync(App app)
    {
        if (_dbContext.Apps.Any(e => e.Name == app.Name))
        {
            throw new UserFriendlyException(_i18N.T("Application name already exists!"));
        }

        await _dbContext.Apps.AddAsync(app);
        await _dbContext.SaveChangesAsync();

        return app;
    }

    public async Task AddEnvironmentClusterProjectAppsAsync(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps)
    {
        await _dbContext.EnvironmentClusterProjectApps.AddRangeAsync(environmentClusterProjectApps);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(int Id)
    {
        var app = _dbContext.Apps.FirstOrDefault(app => app.Id == Id);
        if (app != null)
        {
            _dbContext.Apps.Remove(app);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveEnvironmentClusterProjectApps(int appId, IEnumerable<int> envClusterProjectIds)
    {
        var projectApp = await _dbContext.EnvironmentClusterProjectApps.Where(app => app.AppId == appId && envClusterProjectIds.Contains(app.EnvironmentClusterProjectId)).ToListAsync();
        if (projectApp.Any())
        {
            _dbContext.EnvironmentClusterProjectApps.RemoveRange(projectApp);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveEnvironmentClusterProjectApps(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps)
    {
        if (environmentClusterProjectApps.Any())
        {
            _dbContext.EnvironmentClusterProjectApps.RemoveRange(environmentClusterProjectApps);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<App> GetAsync(int Id)
    {
        var app = await _dbContext.Apps.FindAsync(Id);

        return app ?? throw new UserFriendlyException(_i18N.T("Application information does not exist!"));
    }

    public async Task<App> GetByIdenityAsync(string identity)
    {
        var app = await _dbContext.Apps.FirstOrDefaultAsync(app => app.Identity == identity);

        return app ?? throw new UserFriendlyException(_i18N.T("Application information does not exist!"));
    }

    public async Task<List<App>> GetByAppTypesAsync(List<AppTypes> appTypes)
    {
        var app = await _dbContext.Apps.Where(app => appTypes.Contains(app.Type)).ToListAsync();

        return app;
    }

    public async Task<List<App>> GetListAsync()
    {
        var apps = await _dbContext.Apps.ToListAsync();

        return apps;
    }

    public async Task<List<App>> GetListByProjectIdAsync(IEnumerable<int> projectIds)
    {
        var result = await (from environmentClusterProject in _dbContext.EnvironmentClusterProjects.Where(project => projectIds.Contains(project.ProjectId))
                            join environmentClusterProjectApp in _dbContext.EnvironmentClusterProjectApps on environmentClusterProject.Id equals environmentClusterProjectApp.EnvironmentClusterProjectId
                            join app in _dbContext.Apps on environmentClusterProjectApp.AppId equals app.Id
                            select app)
                            .Distinct()
                            .ToListAsync();
        return result;
    }

    public async Task<List<(EnvironmentClusterProjectApp EnvironmentClusterProjectApp, int ProjectId, string ClusterName, string EnvironmentName, string EnvColor, EnvironmentCluster)>> GetEnvironmentAndClusterNamesByAppIds(IEnumerable<int> appIds)
    {
        var result = await (from environmentClusterProjectApp in _dbContext.EnvironmentClusterProjectApps.Where(environmentClusterProjectApp => appIds.Contains(environmentClusterProjectApp.AppId))
                            join environmentClusterProject in _dbContext.EnvironmentClusterProjects on environmentClusterProjectApp.EnvironmentClusterProjectId equals environmentClusterProject.Id
                            join environmentCluster in _dbContext.EnvironmentClusters on environmentClusterProject.EnvironmentClusterId equals environmentCluster.Id
                            join environment in _dbContext.Environments on environmentCluster.EnvironmentId equals environment.Id
                            join cluster in _dbContext.Clusters on environmentCluster.ClusterId equals cluster.Id
                            select new ValueTuple<EnvironmentClusterProjectApp, int, string, string, string, EnvironmentCluster>
                            (
                                environmentClusterProjectApp,
                                environmentClusterProject.ProjectId,
                                cluster.Name,
                                environment.Name,
                                environment.Color,
                                environmentCluster)
                            )
                            .ToListAsync();

        return result;
    }

    public async Task<List<EnvironmentClusterProjectApp>> GetEnvironmentClusterProjectAppsAsync(int environmentClusterProjectId, int appId)
    {
        var result = await _dbContext.EnvironmentClusterProjectApps.Where(app => app.EnvironmentClusterProjectId == environmentClusterProjectId && app.AppId == appId).ToListAsync();

        return result;
    }

    public async Task<List<EnvironmentClusterProjectApp>> GetEnvironmentClusterProjectAppsByAppId(int appId)
    {
        var result = await _dbContext.EnvironmentClusterProjectApps.Where(ecpa => ecpa.AppId == appId).ToListAsync();

        return result;
    }

    public async Task<List<(int ProjectId, App App, EnvironmentClusterProjectApp EnvironmentClusterProjectApp)>> GetAppByEnvNameAndProjectIdsAsync(string envName, IEnumerable<int> projectIds)
    {
        var result = await (from environmentClusterProjectApp in _dbContext.EnvironmentClusterProjectApps
                            join environmentClusterProject in _dbContext.EnvironmentClusterProjects.Where(project => projectIds.Contains(project.ProjectId)) on environmentClusterProjectApp.EnvironmentClusterProjectId equals environmentClusterProject.Id
                            join environmentCluster in _dbContext.EnvironmentClusters on environmentClusterProject.EnvironmentClusterId equals environmentCluster.Id
                            join environment in _dbContext.Environments.Where(env => env.Name == envName) on environmentCluster.EnvironmentId equals environment.Id
                            join app in _dbContext.Apps on environmentClusterProjectApp.AppId equals app.Id
                            select new ValueTuple<int, App, EnvironmentClusterProjectApp>
                            (environmentClusterProject.ProjectId, app, environmentClusterProjectApp))
                            .ToListAsync();

        return result;
    }

    public async Task UpdateAsync(App app)
    {
        _dbContext.Apps.Update(app);

        await _dbContext.SaveChangesAsync();
    }

    public async Task IsExistedApp(string name, string identity, List<int> environmentClusterProjectIds, params int[] excludeAppIds)
    {
        var result = await (from project in _dbContext.Projects
                            join ecp in _dbContext.EnvironmentClusterProjects on project.Id equals ecp.ProjectId
                            join envCluster in _dbContext.EnvironmentClusters on ecp.EnvironmentClusterId equals envCluster.Id
                            join env in _dbContext.Environments on envCluster.EnvironmentId equals env.Id
                            join cluster in _dbContext.Clusters on envCluster.ClusterId equals cluster.Id
                            join ecpa in _dbContext.EnvironmentClusterProjectApps on ecp.Id equals ecpa.EnvironmentClusterProjectId
                            join app in _dbContext.Apps.Where(app => (app.Name == name || app.Identity.ToLower() == identity) && !excludeAppIds.Contains(app.Id)) on ecpa.AppId equals app.Id
                            select new
                            {
                                EnvironmentName = env.Name,
                                ClusterName = cluster.Name,
                                ProjedctName = project.Name
                            }).FirstOrDefaultAsync();

        if (result != null)
        {
            throw new UserFriendlyException(_i18N.T("Application name or ID already exists!"));
        }
    }
}
