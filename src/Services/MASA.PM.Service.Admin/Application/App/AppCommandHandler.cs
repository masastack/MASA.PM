// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

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
        }

        [EventHandler]
        public async Task RemoveAppAsync(DeleteAppCommand command)
        {
            var envClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectId);
            //await _appRepository.DeleteAsync(command.AppId);
            await _appRepository.RemoveEnvironmentClusterProjectApps(command.AppId, envClusterProjects.Select(ecp => ecp.Id));
        }
    }
}
