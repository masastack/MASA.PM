namespace MASA.PM.Service.Admin.Infrastructure.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly PMDbContext _dbContext;

        public AppRepository(PMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<App> AddAsync(App app)
        {
            if (_dbContext.Apps.Any(e => e.Name == app.Name))
            {
                throw new UserFriendlyException("应用名称已存在！");
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

            return app ?? throw new UserFriendlyException("应用信息不存在！");
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

        public async Task<List<(int AppId, int ProjectId, string ClusterName, string EnvironmentName, EnvironmentCluster)>> GetEnvironmentAndClusterNamesByAppIds(IEnumerable<int> appIds)
        {
            var result = await (from environmentClusterProjectApp in _dbContext.EnvironmentClusterProjectApps.Where(environmentClusterProjectApp => appIds.Contains(environmentClusterProjectApp.AppId))
                                join environmentClusterProject in _dbContext.EnvironmentClusterProjects on environmentClusterProjectApp.EnvironmentClusterProjectId equals environmentClusterProject.Id
                                join environmentCluster in _dbContext.EnvironmentClusters on environmentClusterProject.EnvironmentClusterId equals environmentCluster.Id
                                join environment in _dbContext.Environments on environmentCluster.EnvironmentId equals environment.Id
                                join cluster in _dbContext.Clusters on environmentCluster.ClusterId equals cluster.Id
                                select new ValueTuple<int, int, string, string, EnvironmentCluster>
                                (environmentClusterProjectApp.AppId, environmentClusterProject.ProjectId, cluster.Name, environment.Name, environmentCluster)
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
                                join app in _dbContext.Apps.Where(app => app.Name == name || app.Identity.ToLower() == identity && !excludeAppIds.Contains(app.Id)) on ecpa.AppId equals app.Id
                                select new
                                {
                                    EnvironmentName = env.Name,
                                    ClusterName = cluster.Name,
                                    ProjedctName = project.Name
                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                throw new UserFriendlyException($"应用名或ID已存在！");
            }
        }
    }
}
