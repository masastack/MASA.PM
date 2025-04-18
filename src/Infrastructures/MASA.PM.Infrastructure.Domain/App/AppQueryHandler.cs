﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.App;

internal class AppQueryHandler
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
            Creator = app.Creator,
            CreationTime = app.CreationTime,
            ModificationTime = app.ModificationTime,
            Modifier = app.Modifier,
            ResponsibilityUserIds = app.ResponsibilityUsers?.Select(m => m.UserId).ToList()
        };

        if (query.IsHaveEnvironmentClusterInfo)
        {
            List<(EnvironmentClusterProjectApp EnvironmentClusterProjectApp,
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
    public async Task GetAppAsync(AppByIdentityQuery query)
    {
        var app = await _appRepository.GetByIdenityAsync(query.identity);
        query.Result = new AppDto
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
            ResponsibilityUserIds = app.ResponsibilityUsers?.Select(m => m.UserId).ToList()
        };
    }

    [EventHandler]
    public async Task GetAppListAsync(AppsQuery query)
    {
        if (query.ProjectIds.Any())
        {
            List<Shared.Entities.App> apps = await _appRepository.GetListByProjectIdAsync(query.ProjectIds);
            List<(EnvironmentClusterProjectApp EnvironmentClusterProjectApp,
                 int ProjectId,
                 string ClusterName,
                 string EnvName,
                 string EnvColor,
                 EnvironmentCluster EnvCluster)> appProjectEnvClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(apps.Select(app => app.Id));
            var appEnvironmentClusters = appProjectEnvClusters
                  .Where(appProjectEnvCluster => query.ProjectIds.Contains(appProjectEnvCluster.ProjectId))
                  .Select(appProjectEnvCluster => new AppEnvironmentClusterDto
                  {
                      AppId = appProjectEnvCluster.EnvironmentClusterProjectApp.AppId,
                      ProjectId = appProjectEnvCluster.ProjectId,
                      EnvironmentCluster = new EnvironmentClusterDto
                      {
                          Id = appProjectEnvCluster.EnvCluster.Id,
                          EnvironmentName = appProjectEnvCluster.EnvName,
                          EnvironmentColor = appProjectEnvCluster.EnvColor,
                          ClusterName = appProjectEnvCluster.ClusterName,
                          AppURL = appProjectEnvCluster.EnvironmentClusterProjectApp.AppURL,
                          AppSwaggerURL = appProjectEnvCluster.EnvironmentClusterProjectApp.AppSwaggerURL
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
                Creator = appEnvironmentCluster.app.Creator,
                CreationTime = appEnvironmentCluster.app.CreationTime,
                ModificationTime = appEnvironmentCluster.app.ModificationTime,
                Modifier = appEnvironmentCluster.app.Modifier,
                ResponsibilityUserIds = appEnvironmentCluster.app.ResponsibilityUsers?.Select(r => r.UserId).ToList(),
                EnvironmentClusters = appEnvironmentCluster.environmentClusters.Select(envCluster => new EnvironmentClusterDto
                {
                    Id = envCluster.EnvironmentCluster.Id,
                    EnvironmentName = envCluster.EnvironmentCluster.EnvironmentName,
                    EnvironmentColor = envCluster.EnvironmentCluster.EnvironmentColor,
                    ClusterName = envCluster.EnvironmentCluster.ClusterName,
                    AppURL = envCluster.EnvironmentCluster.AppURL,
                    AppSwaggerURL = envCluster.EnvironmentCluster.AppSwaggerURL
                }).ToList()
            }).OrderByDescending(app => app.ModificationTime)
            .ToList();
        }
        else
        {
            List<Shared.Entities.App> apps = await _appRepository.GetListAsync();
            //query.Result = apps.Select(app => new AppDto
            //{
            //    Name = app.Name,
            //    Description = app.Description,
            //    Id = app.Id,
            //    Identity = app.Identity,
            //    Type = app.Type,
            //    ServiceType = app.ServiceType,
            //    Creator = app.Creator,
            //    CreationTime = app.CreationTime,
            //    ModificationTime = app.ModificationTime,
            //    Modifier = app.Modifier,
            //    ResponsibilityUserIds = app.ResponsibilityUsers?.Select(x => x.UserId).ToList()
            //}).OrderByDescending(app => app.ModificationTime)
            //.ToList();

            List<(EnvironmentClusterProjectApp EnvironmentClusterProjectApp,
                  int ProjectId,
                  string ClusterName,
                  string EnvName,
                  string EnvColor,
                  EnvironmentCluster EnvCluster)> appProjectEnvClusters = await _appRepository.GetEnvironmentAndClusterNamesByAppIds(apps.Select(app => app.Id));

            var appEnvironmentClusters = appProjectEnvClusters
                .Select(appProjectEnvCluster => new AppEnvironmentClusterDto
                {
                    AppId = appProjectEnvCluster.EnvironmentClusterProjectApp.AppId,
                    ProjectId = appProjectEnvCluster.ProjectId,
                    EnvironmentCluster = new EnvironmentClusterDto
                    {
                        Id = appProjectEnvCluster.EnvCluster.Id,
                        EnvironmentName = appProjectEnvCluster.EnvName,
                        EnvironmentColor = appProjectEnvCluster.EnvColor,
                        ClusterName = appProjectEnvCluster.ClusterName,
                        AppURL = appProjectEnvCluster.EnvironmentClusterProjectApp.AppURL,
                        AppSwaggerURL = appProjectEnvCluster.EnvironmentClusterProjectApp.AppSwaggerURL
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
                Creator = appEnvironmentCluster.app.Creator,
                CreationTime = appEnvironmentCluster.app.CreationTime,
                ModificationTime = appEnvironmentCluster.app.ModificationTime,
                Modifier = appEnvironmentCluster.app.Modifier,
                ResponsibilityUserIds = appEnvironmentCluster.app.ResponsibilityUsers?.Select(x => x.UserId).ToList(),
                EnvironmentClusters = appEnvironmentCluster.environmentClusters.Select(envCluster => new EnvironmentClusterDto
                {
                    Id = envCluster.EnvironmentCluster.Id,
                    EnvironmentName = envCluster.EnvironmentCluster.EnvironmentName,
                    EnvironmentColor = envCluster.EnvironmentCluster.EnvironmentColor,
                    ClusterName = envCluster.EnvironmentCluster.ClusterName,
                    AppURL = envCluster.EnvironmentCluster.AppURL,
                    AppSwaggerURL = envCluster.EnvironmentCluster.AppSwaggerURL
                }).ToList()
            }).OrderByDescending(app => app.ModificationTime)
            .ToList();
        }
    }

    [EventHandler]
    public async Task GetAppAsync(AppByTypesQuery query)
    {
        var app = await _appRepository.GetByAppTypesAsync(query.AppTypes.ToList());
        query.Result = app.Select(app => new AppDto
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
            ResponsibilityUserIds = app.ResponsibilityUsers?.Select(m => m.UserId).ToList()
        }).ToList();
    }
}
