using MASA.PM.Service.Admin.Application.App.Commands;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class AppCommandHandler
    {
        private readonly IAppRepository _appRepository;
        private readonly IProjectRepository _projectRepository;

        public AppCommandHandler(IAppRepository appRepository, IProjectRepository projectRepository)
        {
            _appRepository = appRepository;
            _projectRepository = projectRepository;
        }

        [EventHandler]
        public async Task AddAppAsync(AddAppCommand command)
        {
            var appModel = command.AppModel;
            var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(appModel.EnvironmentClusterIds, appModel.ProjectId);

            await _appRepository.IsExistedAppName(command.AppModel.Name, envClusterProjectIds);

            var app = await _appRepository.AddAsync(new Infrastructure.Entities.App
            {
                Name = appModel.Name,
                Type = appModel.Type,
                ServiceType = appModel.ServiceType,
                SwaggerUrl = appModel.SwaggerUrl,
                Url = appModel.Url,
                Creator = appModel.ActionUserId,
                Modifier = appModel.ActionUserId,
                Identity = appModel.Identity,
                Description = appModel.Description,
                IsDeleted = false
            });

            var environmentClusterProjectApps = envClusterProjectIds.Select(environmentClusterProjectId => new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = environmentClusterProjectId,
                AppId = app.Id,
                Creator = appModel.ActionUserId,
                Modifier = appModel.ActionUserId,
                IsDeleted = false
            });
            await _appRepository.AddEnvironmentClusterProjectAppsAsync(environmentClusterProjectApps);
        }

        [EventHandler]
        public async Task AddRelationAppAsync(AddRelationAppCommand command)
        {
            var relationApp = command.RelationAppModel;

            var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(new List<int> { relationApp.EnvironmentClusterId }, relationApp.ProjectId);
            var envClusterProjectApp = new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = envClusterProjectIds[0],
                AppId = relationApp.AppId
            };

            var envClusterProjectApps = await _appRepository.GetEnvironmentClusterProjectAppsAsync(envClusterProjectIds[0], relationApp.AppId);
            if (envClusterProjectApps.Any())
            {
                throw new Exception("该应用已存在");
            }
            await _appRepository.AddEnvironmentClusterProjectAppsAsync(new List<EnvironmentClusterProjectApp> { envClusterProjectApp });
        }

        [EventHandler]
        public async Task UpdateAppAsync(UpdateAppCommand command)
        {
            var appModel = command.UpdateAppModel;
            var appEntity = await _appRepository.GetAsync(appModel.Id);

            var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(appModel.EnvironmentClusterIds, appModel.ProjectId);
            if (appEntity.Name != appModel.Name)
            {
                await _appRepository.IsExistedAppName(appModel.Name, envClusterProjectIds, appModel.Id);
            }

            appEntity.Name = appModel.Name;
            appEntity.SwaggerUrl = appModel.SwaggerUrl;
            appEntity.Url = appModel.Url;
            appEntity.Modifier = appModel.ActionUserId;
            appEntity.Description = appModel.Description;
            appEntity.ModificationTime = DateTime.Now;

            await _appRepository.UpdateAsync(appEntity);

            var envClusterProjectApps = await _appRepository.GetEnvironmentClusterProjectAppsByAppId(appModel.Id);
            await _appRepository.DeleteEnvironmentClusterProjectApps(envClusterProjectApps);
            var environmentClusterProjectApps = envClusterProjectIds.Select(environmentClusterProjectId => new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = environmentClusterProjectId,
                AppId = appModel.Id,
                Creator = appModel.ActionUserId,
                Modifier = appModel.ActionUserId,
                IsDeleted = false
            });
            await _appRepository.AddEnvironmentClusterProjectAppsAsync(environmentClusterProjectApps);
        }

        [EventHandler]
        public async Task DeleteAppAsync(DeleteAppCommand command)
        {
            await _appRepository.DeleteAsync(command.AppId);
            await _appRepository.DeleteEnvironmentClusterProjectAppsByAppIdAsync(command.AppId);
        }
    }
}
