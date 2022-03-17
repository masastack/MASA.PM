using MASA.PM.Service.Admin.Application.App.Queries;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class AppQueryHandler
    {
        private readonly IAppRepository _appRepository;

        public AppQueryHandler(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [EventHandler]
        public async Task AppQueryHandle(AppQuery query)
        {
            var app = await _appRepository.GetAsync(query.AppId);
            query.Result = new AppViewModel
            {
                Name = app.Name,
                Description = app.Description,
                Id = app.Id,
                Identity = app.Identity,
                Type = app.Type,
                ServiceType = app.ServiceType,
                Url = app.Url,
                SwaggerUrl = app.SwaggerUrl,
                Creator = app.Creator,
                CreationTime = app.CreationTime,
                ModificationTime = app.ModificationTime,
                Modifier = app.Modifier,
            };

            if (query.IsHaveEnvironmentClusterInfo)
            {
                List<(int AppId,
                    int ProjectId,
                    string ClusterName,
                    string EnvName,
                    EnvironmentCluster EnvCluster)>
                appProjectEnvClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(new List<int>() { app.Id });

                query.Result.EnvironmentClusters = appProjectEnvClusters.Select(appProjectEnvCluster => new AppEnvironmentClusterViewModel
                {
                    AppId = appProjectEnvCluster.AppId,
                    ProjectId = appProjectEnvCluster.ProjectId,
                    EnvironmentCluster = new EnvironmentClusterViewModel
                    {
                        Id = appProjectEnvCluster.EnvCluster.Id,
                        EnvironmentName = appProjectEnvCluster.EnvName,
                        ClusterName = appProjectEnvCluster.ClusterName
                    }
                }).ToList();
            }
        }

        [EventHandler]
        public async Task AppListQueryHandle(AppsQuery query)
        {
            if (query.ProjectIds.Any())
            {
                var apps = await _appRepository.GetListByProjectIdAsync(query.ProjectIds);
                List<(int AppId,
                    int ProjectId,
                    string ClusterName,
                    string EnvName,
                    EnvironmentCluster EnvCluster)>
                appProjectEnvClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(apps.Select(app => app.Id));
                var appEnvironmentClusters = appProjectEnvClusters.Select(appProjectEnvCluster => new AppEnvironmentClusterViewModel
                {
                    AppId = appProjectEnvCluster.AppId,
                    ProjectId = appProjectEnvCluster.ProjectId,
                    EnvironmentCluster = new EnvironmentClusterViewModel
                    {
                        Id = appProjectEnvCluster.EnvCluster.Id,
                        EnvironmentName = appProjectEnvCluster.EnvName,
                        ClusterName = appProjectEnvCluster.ClusterName
                    }
                }).ToList();
                var environmentClusterGroup = appEnvironmentClusters.GroupBy(c => new { c.ProjectId, c.AppId }).ToList();

                var result = apps.Join(
                        environmentClusterGroup,
                        app => app.Id,
                        environmentCluster => environmentCluster.Key.AppId,
                        (app, environmentClusters) => new { app, environmentClusters });

                query.Result = result.Select(appEnvironmentCluster => new AppViewModel
                {
                    ProjectId = appEnvironmentCluster.environmentClusters.Key.ProjectId,
                    Name = appEnvironmentCluster.app.Name,
                    Description = appEnvironmentCluster.app.Description,
                    Id = appEnvironmentCluster.app.Id,
                    Identity = appEnvironmentCluster.app.Identity,
                    Type = appEnvironmentCluster.app.Type,
                    ServiceType = appEnvironmentCluster.app.ServiceType,
                    Url = appEnvironmentCluster.app.Url,
                    SwaggerUrl = appEnvironmentCluster.app.SwaggerUrl,
                    Creator = appEnvironmentCluster.app.Creator,
                    CreationTime = appEnvironmentCluster.app.CreationTime,
                    ModificationTime = appEnvironmentCluster.app.ModificationTime,
                    Modifier = appEnvironmentCluster.app.Modifier,
                    EnvironmentClusters = appEnvironmentCluster.environmentClusters.ToList()
                }).ToList();
            }
            else
            {
                var apps = await _appRepository.GetListAsync();
                query.Result = apps.Select(app => new AppViewModel
                {
                    Name = app.Name,
                    Description = app.Description,
                    Id = app.Id,
                    Identity = app.Identity,
                    Type = app.Type,
                    ServiceType = app.ServiceType,
                    Url = app.Url,
                    SwaggerUrl = app.SwaggerUrl,
                    Creator = app.Creator,
                    CreationTime = app.CreationTime,
                    ModificationTime = app.ModificationTime,
                    Modifier = app.Modifier
                }).ToList();
            }
        }
    }
}
