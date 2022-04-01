using MASA.PM.Service.Admin.Application.App.Commands;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class AppCommandHandler
    {
        private const string _APP_KEY_PREFIX = "masa.pm.app";

        private readonly IAppRepository _appRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IMemoryCacheClient _memoryCacheClient;
        private const string APP_KEYS = "masa.pm.app.keys";
        private const string APP_KEY_PREFIX = "masa.pm.app.key";

        public AppCommandHandler(IAppRepository appRepository, IProjectRepository projectRepository, IEnvironmentRepository environmentRepository, IMemoryCacheClient memoryCacheClient)
        {
            _appRepository = appRepository;
            _projectRepository = projectRepository;
            _environmentRepository = environmentRepository;
            _memoryCacheClient = memoryCacheClient;
        }

        [EventHandler]
        public async Task AddAppAsync(AddAppCommand command)
        {
            var appModel = command.AppModel;
            var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(appModel.EnvironmentClusterIds, appModel.ProjectId);

            await _appRepository.IsExistedApp(appModel.Name, appModel.Identity, envClusterProjectIds);

            var app = await _appRepository.AddAsync(new Infrastructure.Entities.App
            {
                Name = appModel.Name,
                Type = appModel.Type,
                ServiceType = appModel.ServiceType,
                SwaggerUrl = appModel.SwaggerUrl,
                Url = appModel.Url,
                Creator = MasaUser.UserId,
                Modifier = MasaUser.UserId,
                Identity = appModel.Identity,
                Description = appModel.Description,
                IsDeleted = false
            });

            var environmentClusterProjectApps = envClusterProjectIds.Select(environmentClusterProjectId => new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = environmentClusterProjectId,
                AppId = app.Id
            });
            await _appRepository.AddEnvironmentClusterProjectAppsAsync(environmentClusterProjectApps);

            //add redis cache
            var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(appModel.EnvironmentClusterIds);
            await _memoryCacheClient.SetAsync<AppModel>($"{_APP_KEY_PREFIX}.{app.Id}",
                new AppModel(
                    app.Id,
                    app.Name,
                    app.Identity,
                    appModel.ProjectId,
                    envs.Select(env => env.Id))
                );
        }

        [EventHandler]
        public async Task AddRelationAppAsync(AddRelationAppCommand command)
        {
            var relationApp = command.RelationAppModel;

            var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(relationApp.EnvironmentClusterIds, relationApp.ProjectId);
            var envClusterProjectApp = new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = envClusterProjectIds[0],
                AppId = relationApp.AppId
            };

            var envClusterProjectApps = await _appRepository.GetEnvironmentClusterProjectAppsAsync(envClusterProjectIds[0], relationApp.AppId);
            if (envClusterProjectApps.Any())
            {
                throw new UserFriendlyException("该应用已存在");
            }
            await _appRepository.AddEnvironmentClusterProjectAppsAsync(new List<EnvironmentClusterProjectApp> { envClusterProjectApp });

            //update redis cache
            var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(relationApp.EnvironmentClusterIds);
            AppModel app = (await _memoryCacheClient.GetAsync<AppModel>($"{_APP_KEY_PREFIX}.{relationApp.AppId}"))!;
            app.EnvironmentIds.AddRange(envs.Select(env => env.Id));
            await _memoryCacheClient.SetAsync<AppModel>($"{_APP_KEY_PREFIX}.{app.Id}",
                new AppModel(
                    app.Id,
                    app.Name,
                    app.Identity,
                    relationApp.ProjectId,
                    app.EnvironmentIds)
                );
        }

        [EventHandler]
        public async Task UpdateAppAsync(UpdateAppCommand command)
        {
            var appModel = command.UpdateAppModel;
            var appEntity = await _appRepository.GetAsync(appModel.Id);

            var envClusterProjectIds = await _projectRepository.GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(appModel.EnvironmentClusterIds, appModel.ProjectId);
            if (appEntity.Name != appModel.Name)
            {
                await _appRepository.IsExistedApp(appModel.Name, appModel.Identity, envClusterProjectIds, appModel.Id);
            }

            appEntity.Name = appModel.Name;
            appEntity.SwaggerUrl = appModel.SwaggerUrl;
            appEntity.Url = appModel.Url;
            appEntity.Modifier = MasaUser.UserId;
            appEntity.Description = appModel.Description;
            appEntity.ModificationTime = DateTime.Now;

            await _appRepository.UpdateAsync(appEntity);

            var envClusterProjectApps = await _appRepository.GetEnvironmentClusterProjectAppsByAppId(appModel.Id);
            await _appRepository.RemoveEnvironmentClusterProjectApps(envClusterProjectApps);

            var environmentClusterProjectApps = envClusterProjectIds.Select(environmentClusterProjectId => new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = environmentClusterProjectId,
                AppId = appModel.Id
            });
            await _appRepository.AddEnvironmentClusterProjectAppsAsync(environmentClusterProjectApps);

            //update redis cache
            var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(appModel.EnvironmentClusterIds);
            await _memoryCacheClient.SetAsync<AppModel>($"{_APP_KEY_PREFIX}.{appEntity.Id}",
                new AppModel(
                    appEntity.Id,
                    appEntity.Name,
                    appEntity.Identity,
                    appModel.ProjectId,
                    envs.Select(env => env.Id))
                );
        }

        [EventHandler]
        public async Task RemoveAppAsync(DeleteAppCommand command)
        {
            var envClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectId);
            //await _appRepository.DeleteAsync(command.AppId);
            await _appRepository.RemoveEnvironmentClusterProjectApps(command.AppId, envClusterProjects.Select(ecp => ecp.Id));

            //remove redis cache
            var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(envClusterProjects.Select(envClusterProject => envClusterProject.EnvironmentClusterId));
            AppModel app = (await _memoryCacheClient.GetAsync<AppModel>($"{_APP_KEY_PREFIX}.{command.AppId}"))!;
            app.EnvironmentIds.RemoveAll(envId => envs.Select(env => env.Id).Contains(envId));
            await _memoryCacheClient.SetAsync<AppModel>($"{_APP_KEY_PREFIX}.{command.AppId}", app);
        }
    }
}
