﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

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
            var envClusterProjects = await _projectRepository
                .GetEnvironmentClusterProjectsByEnvClusterIdsAndProjectId(
                    appModel.EnvironmentClusterInfo.Select(c => c.EnvironmentClusterId), appModel.ProjectId);

            await _appRepository.IsExistedApp(
                appModel.Name, appModel.Identity, envClusterProjects.Select(e => e.Id).ToList());

            var app = await _appRepository.AddAsync(new Infrastructure.Entities.App
            {
                Name = appModel.Name,
                Type = appModel.Type,
                ServiceType = appModel.ServiceType,
                Identity = appModel.Identity,
                Description = appModel.Description
            });

            List<EnvironmentClusterProjectApp> environmentClusterProjectApps = new();
            foreach (var envClusterProject in envClusterProjects)
            {
                var (EnvironmentClusterId, Url, SwaggerUrl) = appModel.EnvironmentClusterInfo.FirstOrDefault(e => e.EnvironmentClusterId == envClusterProject.Id);
                environmentClusterProjectApps.Add(new EnvironmentClusterProjectApp
                {
                    EnvironmentClusterProjectId = envClusterProject.Id,
                    AppId = app.Id,
                    AppURL = Url,
                    AppSwaggerURL = SwaggerUrl
                });
            }

            await _appRepository.AddEnvironmentClusterProjectAppsAsync(environmentClusterProjectApps);
        }

        [EventHandler]
        public async Task AddRelationAppAsync(AddRelationAppCommand command)
        {
            var relationApp = command.RelationAppModel;

            var envClusterProjects = await _projectRepository
                .GetEnvironmentClusterProjectsByEnvClusterIdsAndProjectId(
                    relationApp.EnvironmentClusterIds, relationApp.ProjectId);
            var envClusterProjectApp = new EnvironmentClusterProjectApp
            {
                EnvironmentClusterProjectId = envClusterProjects[0].Id,
                AppId = relationApp.AppId
            };

            var envClusterProjectApps = await _appRepository.GetEnvironmentClusterProjectAppsAsync(envClusterProjects[0].Id, relationApp.AppId);
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

            var envClusterProjects = await _projectRepository
                .GetEnvironmentClusterProjectsByEnvClusterIdsAndProjectId(
                    appModel.EnvironmentClusterInfo.Select(c => c.EnvironmentClusterId), appModel.ProjectId);
            if (appEntity.Name != appModel.Name)
            {
                await _appRepository.IsExistedApp(
                    appModel.Name, appModel.Identity, envClusterProjects.Select(e => e.Id).ToList(), appModel.Id);
            }

            appEntity.Name = appModel.Name;
            appEntity.Description = appModel.Description;

            await _appRepository.UpdateAsync(appEntity);

            var envClusterProjectApps = await _appRepository.GetEnvironmentClusterProjectAppsByAppId(appModel.Id);
            await _appRepository.RemoveEnvironmentClusterProjectApps(envClusterProjectApps);

            List<EnvironmentClusterProjectApp> environmentClusterProjectApps = new();
            foreach (var envClusterProject in envClusterProjects)
            {
                var (EnvironmentClusterId, Url, SwaggerUrl) = appModel.EnvironmentClusterInfo.FirstOrDefault(e => e.EnvironmentClusterId == envClusterProject.Id);
                environmentClusterProjectApps.Add(new EnvironmentClusterProjectApp
                {
                    EnvironmentClusterProjectId = envClusterProject.Id,
                    AppId = appModel.Id,
                    AppURL = Url,
                    AppSwaggerURL = SwaggerUrl
                });
            }

            await _appRepository.AddEnvironmentClusterProjectAppsAsync(environmentClusterProjectApps);
        }

        [EventHandler]
        public async Task RemoveAppAsync(RemoveAppCommand command)
        {
            await _appRepository.RemoveAsync(command.AppId);
        }
    }
}
