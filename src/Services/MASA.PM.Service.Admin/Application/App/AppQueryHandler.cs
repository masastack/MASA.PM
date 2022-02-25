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
        public async Task AppListQueryHandle(AppsQuery query)
        {
            var apps = await _appRepository.GetListByProjectIdAsync(query.ProjectIds);
            var environmentClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(apps.Select(app => app.Id));
            var environmentClusterGroup = environmentClusters.GroupBy(c => new { c.ProjectId, c.AppId }).ToList();

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
                Creator = appEnvironmentCluster.app.Creator,
                CreationTime = appEnvironmentCluster.app.CreationTime,
                ModificationTime = appEnvironmentCluster.app.ModificationTime,
                Modifier = appEnvironmentCluster.app.Modifier,
                EnvironmentClusters = appEnvironmentCluster.environmentClusters.ToList()
            }).ToList();
        }
    }
}
