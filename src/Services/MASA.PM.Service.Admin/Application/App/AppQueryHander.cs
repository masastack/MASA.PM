using MASA.PM.Service.Admin.Application.App.Queries;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class AppQueryHander
    {
        private readonly IAppRepository _appRepository;

        public AppQueryHander(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [EventHandler]
        public async Task AppQueryHande(AppQuery query)
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
                Creator = app.Creator,
                CreationTime = app.CreationTime,
                ModificationTime = app.ModificationTime,
                Modifier = app.Modifier,
            };

            if (query.IsHaveEnvironmentClusterInfo)
            {
                var environmentClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(new List<int>() { app.Id });
                query.Result.EnvironmentClusters = environmentClusters;
            }
        }

        [EventHandler]
        public async Task AppListQueryHande(AppsQuery query)
        {
            var apps = await _appRepository.GetListByProjectIdAsync(query.ProjectId);
            var environmentClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(apps.Select(app => app.Id));
            var environmentClusterGroup = environmentClusters.GroupBy(c => c.AppId).ToList();

            var result = apps.Join(
                    environmentClusterGroup,
                    app => app.Id,
                    environmentCluster => environmentCluster.Key,
                    (app, environmentClusters) => new { app, environmentClusters });

            query.Result = result.Select(appEnvironmentCluster => new AppViewModel
            {
                Name = appEnvironmentCluster.app.Name,
                Description = appEnvironmentCluster.app.Description,
                Id = appEnvironmentCluster.app.Id,
                Identity = appEnvironmentCluster.app.Identity,
                Type = appEnvironmentCluster.app.Type,
                ServiceType = appEnvironmentCluster.app.ServiceType,
                Creator = appEnvironmentCluster.app.Creator,
                CreationTime = appEnvironmentCluster.app.CreationTime,
                ModificationTime = appEnvironmentCluster.app.ModificationTime,
                Modifier = appEnvironmentCluster.app.Modifier,
                EnvironmentClusters = appEnvironmentCluster.environmentClusters.ToList()
            }).ToList();
        }
    }
}
