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
        public async Task UpdateClusterAsync(UpdateAppCommand command)
        {
            var appModel = command.UpdateAppModel;
            var appEntity = await _appRepository.GetAsync(appModel.Id);

            if (appEntity.Name != appModel.Name)
            {
                var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(appModel.EnvironmentClusterIds, appModel.ProjectId);
                await _appRepository.IsExistedAppName(appModel.Name, envClusterProjectIds, appModel.Id);
            }

            appEntity.Name = appModel.Name;
            appEntity.SwaggerUrl = appModel.SwaggerUrl;
            appEntity.Url = appModel.Url;
            appEntity.Modifier = appModel.ActionUserId;
            appEntity.Description = appModel.Description;
            appEntity.ModificationTime = DateTime.Now;

            await _appRepository.UpdateAsync(appEntity);
        }

        [EventHandler]
        public async Task DeleteClusterAsync(DeleteAppCommand command)
        {
            await _appRepository.DeleteAsync(command.AppId);
            await _appRepository.DeleteEnvironmentClusterProjectAppsByAppIdAsync(command.AppId);
        }
    }
}
