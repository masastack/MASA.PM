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
            if (_dbContext.Apps.Any(e => e.Name.ToLower() == app.Name.ToLower()))
            {
                throw new Exception("应用名称已存在！");
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

        public async Task DeleteAsync(int Id)
        {
            var app = _dbContext.Apps.FirstOrDefault(app => app.Id == Id);
            if (app != null)
            {
                _dbContext.Apps.Remove(app);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteEnvironmentClusterProjectAppsByAppIdAsync(int Id)
        {
            var projectApp = await _dbContext.EnvironmentClusterProjectApps.Where(app => app.AppId == Id).ToListAsync();
            if (projectApp.Any())
            {
                _dbContext.EnvironmentClusterProjectApps.RemoveRange(projectApp);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteEnvironmentClusterProjectApps(IEnumerable<EnvironmentClusterProjectApp> environmentClusterProjectApps)
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

            return app ?? throw new Exception("应用信息不存在！");
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

        public async Task<List<AppEnvironmentClusterViewModel>> GetEnvironmentAndClusterNamesByAppIds(IEnumerable<int> appIds)
        {
            var result = await (from environmentClusterProjectApp in _dbContext.EnvironmentClusterProjectApps.Where(environmentClusterProjectApp => appIds.Contains(environmentClusterProjectApp.AppId))
                                join environmentClusterProject in _dbContext.EnvironmentClusterProjects on environmentClusterProjectApp.EnvironmentClusterProjectId equals environmentClusterProject.Id
                                join environmentCluster in _dbContext.EnvironmentClusters on environmentClusterProject.EnvironmentClusterId equals environmentCluster.Id
                                join environment in _dbContext.Environments on environmentCluster.EnvironmentId equals environment.Id
                                join cluster in _dbContext.Clusters on environmentCluster.ClusterId equals cluster.Id
                                select new AppEnvironmentClusterViewModel
                                {
                                    AppId = environmentClusterProjectApp.AppId,
                                    ProjectId = environmentClusterProject.ProjectId,
                                    EnvironmentClusterId = environmentCluster.Id,
                                    EnvironmentClusterName = $"{environment.Name}/{cluster.Name}"
                                })

                         .ToListAsync();

            return result;
        }

        public async Task<List<App>> GetListByEnvironmentClusterProjectIdAsync(int environmentClusterProjectId)
        {
            var result = await (from projectApp in _dbContext.EnvironmentClusterProjectApps.Where(ap => ap.EnvironmentClusterProjectId == environmentClusterProjectId)
                                join app in _dbContext.Apps on projectApp.AppId equals app.Id
                                select app)
                                .ToListAsync();

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

        public async Task IsExistedAppName(string name, List<int> environmentClusterProjectIds, params int[] excludeAppIds)
        {
            var result = await (from project in _dbContext.Projects.Where(project => project.Name.ToLower() == name.ToLower())
                                join ecp in _dbContext.EnvironmentClusterProjects on project.Id equals ecp.ProjectId
                                join ec in _dbContext.EnvironmentClusters on ecp.EnvironmentClusterId equals ec.Id
                                join e in _dbContext.Environments on ec.EnvironmentId equals e.Id
                                join c in _dbContext.Clusters on ec.ClusterId equals c.Id
                                join ecpa in _dbContext.EnvironmentClusterProjectApps on ecp.Id equals ecpa.EnvironmentClusterProjectId
                                join app in _dbContext.Apps.Where(a => !excludeAppIds.Contains(a.Id)) on ecpa.AppId equals app.Id
                                select new
                                {
                                    EnvironmentName = e.Name,
                                    ClusterName = c.Name,
                                    ProjedctName = project.Name
                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                throw new Exception($"应用名[{name}]已在环境[{result.EnvironmentName}]/环境[{result.ClusterName}]中存在！");
            }
        }
    }
}
