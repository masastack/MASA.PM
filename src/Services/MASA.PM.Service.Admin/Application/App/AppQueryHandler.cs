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
        public async Task GetAppAsync(AppQuery query)
        {
            var app = await _appRepository.GetAsync(query.AppId);
            query.Result = new AppDto
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
                    string EnvColor,
                    EnvironmentCluster EnvCluster)>
                appProjectEnvClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(new List<int>() { app.Id });

                query.Result.EnvironmentClusters = appProjectEnvClusters.Select(appProjectEnvCluster => new EnvironmentClusterDto
                {
                    Id = appProjectEnvCluster.EnvCluster.Id,
                    EnvironmentName = appProjectEnvCluster.EnvName,
                    EnvironmentColor = appProjectEnvCluster.EnvColor,
                    ClusterName = appProjectEnvCluster.ClusterName
                }).ToList();
            }
        }

        [EventHandler]
        public async Task GetAppListAsync(AppsQuery query)
        {
            if (query.ProjectIds.Any())
            {
                var apps = await _appRepository.GetListByProjectIdAsync(query.ProjectIds);
                List<(int AppId,
                    int ProjectId,
                    string ClusterName,
                    string EnvName,
                    string EnvColor,
                    EnvironmentCluster EnvCluster)>
                appProjectEnvClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(apps.Select(app => app.Id));
                var appEnvironmentClusters = appProjectEnvClusters.Select(appProjectEnvCluster => new AppEnvironmentClusterDto
                {
                    AppId = appProjectEnvCluster.AppId,
                    ProjectId = appProjectEnvCluster.ProjectId,
                    EnvironmentCluster = new EnvironmentClusterDto
                    {
                        Id = appProjectEnvCluster.EnvCluster.Id,
                        EnvironmentName = appProjectEnvCluster.EnvName,
                        EnvironmentColor = appProjectEnvCluster.EnvColor,
                        ClusterName = appProjectEnvCluster.ClusterName
                    }
                }).ToList();
                var environmentClusterGroup = appEnvironmentClusters.GroupBy(c => new { c.ProjectId, c.AppId }).ToList();

                var result = apps.Join(
                        environmentClusterGroup,
                        app => app.Id,
                        environmentCluster => environmentCluster.Key.AppId,
                        (app, environmentClusters) => new { app, environmentClusters });

                query.Result = result.Select(appEnvironmentCluster => new AppDto
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
                    EnvironmentClusters = appEnvironmentCluster.environmentClusters.Select(envCluster=>new EnvironmentClusterDto
                    {
                        Id = envCluster.EnvironmentCluster.Id,
                        EnvironmentName = envCluster.EnvironmentCluster.EnvironmentName,
                        EnvironmentColor = envCluster.EnvironmentCluster.EnvironmentColor,
                        ClusterName = envCluster.EnvironmentCluster.ClusterName
                    }).ToList()
                }).OrderByDescending(app => app.ModificationTime)
                .ToList();
            }
            else
            {
                var apps = await _appRepository.GetListAsync();
                query.Result = apps.Select(app => new AppDto
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
                }).OrderByDescending(app => app.ModificationTime)
                .ToList();
            }
        }
    }
}
