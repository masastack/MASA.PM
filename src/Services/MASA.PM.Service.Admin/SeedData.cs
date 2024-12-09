// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin;

internal static class SeedData
{
    public static async Task MigrateAsync(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<PmDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    public static async Task SeedDataAsync(this WebApplicationBuilder builder, IMasaStackConfig masaStackConfig)
    {
        var onLineEnvironmentName = builder.Environment.EnvironmentName;
        var serviceProvider = builder.Services.BuildServiceProvider();
        var context = serviceProvider.GetRequiredService<PmDbContext>();

        var environmentRepository = serviceProvider.GetRequiredService<IEnvironmentRepository>();
        var clusterRepository = serviceProvider.GetRequiredService<IClusterRepository>();
        var projectRepository = serviceProvider.GetRequiredService<IProjectRepository>();
        var appRepository = serviceProvider.GetRequiredService<IAppRepository>();

        if (!await context.Set<PM.Infrastructure.Domain.Shared.Entities.Environment>().AnyAsync())
        {
            var initDto = new InitDto
            {
                ClusterName = masaStackConfig.Cluster,
                Environments = new List<AddEnvironmentDto>
                {
                    new ()
                    {
                        Name = masaStackConfig.Environment,
                        Description = masaStackConfig.Environment,
                        Color = "#FF5252"
                    }
                }
            };

            if (!initDto.Environments.Exists(env => env.Name.ToLower().Equals(onLineEnvironmentName.ToLower())))
            {
                initDto.Environments.Add(new AddEnvironmentDto
                {
                    Name = onLineEnvironmentName,
                    Description = onLineEnvironmentName,
                    Color = "#37D7AD"
                });
            }

            await InitEnvironmentAndClusterAsync(initDto, masaStackConfig, environmentRepository, clusterRepository);
        }

        if (!context.Set<Project>().Any())
        {
            var envClusetrIds = context.Set<EnvironmentCluster>().Select(ec => ec.Id).ToList();
            await InitProjectAndAppAsync(masaStackConfig, projectRepository, appRepository, envClusetrIds);
        }
    }

    static async Task InitProjectAndAppAsync(
        IMasaStackConfig masaStackConfig,
        IProjectRepository projectRepository,
        IAppRepository appRepository,
        List<int> envClusetrIds)
    {
        var defaultUserId = masaStackConfig.GetDefaultUserId();
        var defaultTeamId = masaStackConfig.GetDefaultTeamId();

        //project
        var projects = masaStackConfig.GetProjectApps();

        var projectIds = new List<int>();

        foreach (var project in projects)
        {
            var appGroups = new List<(int ProjectId, string ProjectIdentity, int AppId, string App)>();
            var projectEntity = project.Adapt<Project>();
            projectEntity.SetCreatorAndModifier(defaultUserId);
            var newProject = await projectRepository.AddAsync(projectEntity);
            var environmentProjectTeam = new EnvironmentProjectTeam
            {
                TeamId = defaultTeamId,
                ProjectId = newProject.Id,
                EnvironmentName = masaStackConfig.Environment,
            };
            await projectRepository.AddEnvironemtProjectTeamAsync(environmentProjectTeam);

            projectIds.Add(newProject.Id);
            foreach (var app in project.Apps)
            {
                var appEntity = app.Adapt<App>();
                appEntity.SetCreatorAndModifier(defaultUserId);
                var newApp = await appRepository.AddAsync(appEntity);

                appGroups.Add((newProject.Id, newProject.Identity, newApp.Id, newApp.Type switch
                {
                    AppTypes.Service => MasaStackApp.Service.Name,
                    AppTypes.UI => MasaStackApp.WEB.Name,
                    _ => ""
                }));
            }

            foreach (var envClusetrId in envClusetrIds)
            {
                var newEnvironmentClusterProject = await projectRepository.AddEnvironmentClusterProjectAsync(new EnvironmentClusterProject
                {
                    EnvironmentClusterId = envClusetrId,
                    ProjectId = newProject.Id
                });

                var envClusterProjectApps = new List<EnvironmentClusterProjectApp>();
                foreach (var app in appGroups)
                {
                    envClusterProjectApps.Add(new EnvironmentClusterProjectApp
                    {
                        EnvironmentClusterProjectId = newEnvironmentClusterProject.Id,
                        AppId = app.AppId,
                        AppURL = masaStackConfig.GetDomain(new MasaStackProject(default, project.Identity), new MasaStackApp(default, app.App))
                    });
                }
                await appRepository.AddEnvironmentClusterProjectAppsAsync(envClusterProjectApps);
            }
        }
    }

    public static async Task InitEnvironmentAndClusterAsync(
        InitDto initDto,
        IMasaStackConfig masaStackConfig,
        IEnvironmentRepository environmentRepository,
        IClusterRepository clusterRepository)
    {
        var defaultUserId = masaStackConfig.GetDefaultUserId();

        //environment
        var environments = initDto.Environments.Select(e =>
        {
            var env = new PM.Infrastructure.Domain.Shared.Entities.Environment
            {
                Name = e.Name,
                Description = e.Description,
                Color = e.Color
            };
            env.SetCreatorAndModifier(defaultUserId);
            return env;
        });

        var environmentIds = new List<int>();
        foreach (var env in environments)
        {
            var newEnv = await environmentRepository.AddAsync(env);
            environmentIds.Add(newEnv.Id);
        }

        //cluster
        var cluster = new Cluster
        {
            Name = initDto.ClusterName,
            Description = initDto.ClusterDescription
        };
        cluster.SetCreatorAndModifier(defaultUserId);
        await clusterRepository.AddAsync(cluster);

        var envClusters = environmentIds.Select(envId => new EnvironmentCluster
        {
            EnvironmentId = envId,
            ClusterId = cluster.Id,
        });
        foreach (var envCluster in envClusters)
        {
            _ = await environmentRepository.AddEnvironmentClusterAsync(envCluster);
        }
    }
}
