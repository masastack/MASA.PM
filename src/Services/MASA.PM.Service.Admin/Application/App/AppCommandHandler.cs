using MASA.PM.Service.Admin.Application.App.Commands;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class AppCommandHandler
    {
        private readonly IAppRepository _appRepository;

        public AppCommandHandler(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [EventHandler]
        public async Task AddAppAsync(AddAppCommand command)
        {
            var appModel = command.AppModel;
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

            var environmentClusterProjectApps = appModel.EnvironmentClusterProjectIds.Select(environmentClusterProjectId => new EnvironmentClusterProjectApp
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

            await _appRepository.UpdateAsync(new Infrastructure.Entities.App
            {
                Name = appModel.Name,
                SwaggerUrl = appModel.SwaggerUrl,
                Url = appModel.Url,
                Modifier = appModel.ActionUserId,
                Description = appModel.Description,
            });
        }

        [EventHandler]
        public async Task DeleteClusterAsync(DeleteAppCommand command)
        {
            await _appRepository.DeleteAsync(command.AppId);
            await _appRepository.DeleteEnvironmentClusterProjectAppsByAppIdAsync(command.AppId);
        }
    }
}
