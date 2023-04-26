// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Migrations
{
    public static class SeedData
    {
        public static async Task MigrateAsync(this IHost host)
        {
            await using var scope = host.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<PmDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
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

            if (context.Set<Infrastructure.Entities.Environment>().Any())
            {
                return;
            }

            var initDto = new InitDto
            {
                ClusterName = masaStackConfig.Cluster,
                Environments = new List<AddEnvironmentDto> {
                    //new AddEnvironmentDto
                    //{
                    //    Name = "Development",
                    //    Description="开发环境",
                    //    Color = "#FF7D00"
                    //},
                    //new AddEnvironmentDto
                    //{
                    //    Name = "Staging",
                    //    Description="模拟环境",
                    //    Color = "#37A7FF"
                    //},
                    new AddEnvironmentDto
                    {
                        Name = masaStackConfig.Environment,
                        Description= masaStackConfig.Environment + "Environment",
                        Color = "#FF5252"
                    }
                }
            };

            if (!initDto.Environments.Any(e => e.Name.ToLower() == masaStackConfig.Environment.ToLower()))
            {
                initDto.Environments.Add(new AddEnvironmentDto
                {
                    Name = masaStackConfig.Environment,
                    Description = masaStackConfig.Environment,
                    Color = "#FF5252"
                });
            }

            if (!initDto.Environments.Any(env => env.Name.ToLower().Equals(onLineEnvironmentName.ToLower())))
            {
                initDto.Environments.Add(new AddEnvironmentDto
                {
                    Name = onLineEnvironmentName,
                    Description = onLineEnvironmentName,
                    Color = "#37D7AD"
                });
            }

            await InitAsync(
                initDto,
                masaStackConfig,
                environmentRepository,
                clusterRepository,
                projectRepository,
                appRepository);
        }

        public static async Task InitAsync(
            InitDto initDto,
            IMasaStackConfig masaStackConfig,
            IEnvironmentRepository environmentRepository,
            IClusterRepository clusterRepository,
            IProjectRepository projectRepository,
            IAppRepository appRepository)
        {
            var defaultUserId = masaStackConfig.GetDefaultUserId();
            var defaultTeamId = masaStackConfig.GetDefaultTeamId();

            //environment
            var envs = initDto.Environments.Select(e =>
            {
                var env = new Infrastructure.Entities.Environment
                {
                    Name = e.Name,
                    Description = e.Description,
                    Color = e.Color
                };
                env.SetCreatorAndModifier(defaultUserId);
                return env;
            });

            var envEntitis = new List<Infrastructure.Entities.Environment>();
            foreach (var env in envs)
            {
                var newEnv = await environmentRepository.AddAsync(env);
                envEntitis.Add(newEnv);
            }

            //cluster
            var cluster = new Cluster
            {
                Name = initDto.ClusterName,
                Description = initDto.ClusterDescription
            };
            cluster.SetCreatorAndModifier(defaultUserId);
            await clusterRepository.AddAsync(cluster);

            var envClusters = envEntitis.Select(env => new EnvironmentCluster
            {
                EnvironmentId = env.Id,
                ClusterId = cluster.Id,
            });
            var envClusetr = new List<EnvironmentCluster>();
            foreach (var envCluster in envClusters)
            {
                var newEnvCluster = await environmentRepository.AddEnvironmentClusterAsync(envCluster);
                envClusetr.Add(newEnvCluster);
            }

            //project
            var projects = GetProjectApps(masaStackConfig);
            var projectIds = new List<int>();
            var envClusterProject = new List<EnvironmentClusterProject>();
            var appGroups = new List<(int ProjectId, string ProjectIdentity, int AppId, string AppIdentity)>();


            foreach (var project in projects)
            {
                var projectEntity = project.Adapt<Project>();
                projectEntity.TeamId = defaultTeamId;
                projectEntity.SetCreatorAndModifier(defaultUserId);
                var newProject = await projectRepository.AddAsync(projectEntity);

                projectIds.Add(newProject.Id);
                foreach (var app in project.Apps)
                {
                    var appEntity = app.Adapt<App>();
                    appEntity.SetCreatorAndModifier(defaultUserId);
                    var newApp = await appRepository.AddAsync(appEntity);

                    appGroups.Add((newProject.Id, newProject.Identity, newApp.Id, newApp.Identity));
                }

                foreach (var envCluster in envClusetr)
                {
                    var newEnvironmentClusterProject = await projectRepository.AddEnvironmentClusterProjectAsync(new EnvironmentClusterProject
                    {
                        EnvironmentClusterId = envCluster.Id,
                        ProjectId = newProject.Id
                    });

                    var envClusterProjectApps = new List<EnvironmentClusterProjectApp>();
                    foreach (var app in appGroups)
                    {
                        if (app.ProjectId == newProject.Id)
                        {
                            envClusterProjectApps.Add(new EnvironmentClusterProjectApp
                            {
                                EnvironmentClusterProjectId = newEnvironmentClusterProject.Id,
                                AppId = app.AppId,
                                AppURL = masaStackConfig.GetDomain(project.Identity, app.AppIdentity)
                            });
                        }
                    }
                    await appRepository.AddEnvironmentClusterProjectAppsAsync(envClusterProjectApps);
                }
            }


        }

        public static List<AddProjectAppDto> GetProjectApps(IMasaStackConfig masaStackConfig)
        {
            var masaStack = masaStackConfig.GetMasaStack();

            List<AddProjectAppDto> projectApps = new List<AddProjectAppDto>();
            foreach (var service in masaStack)
            {
                if (service == null)
                {
                    continue;
                }

                if (service["id"] == null)
                {
                    continue;
                }

                var id = service["id"]!.ToString();

                AddProjectAppDto projectApp = new AddProjectAppDto
                {
                    Name = service["name"]?.ToString() ?? "",
                    Identity = id,
                    LabelCode = GetLabel(id),
                    TeamId = masaStackConfig.GetDefaultTeamId(),
                    Description = ""
                };

                foreach (var app in service.AsObject())
                {
                    if (app.Key == "id" || app.Key == "name")
                    {
                        continue;
                    }
                    projectApp.Apps.Add(GenAppDto(app));
                }

                projectApps.Add(projectApp);
            }

            return projectApps;
        }

        private static string GetLabel(string projectIdentity)
        {
            //temporary hard code
            var labelDictionary = new Dictionary<string, string>() {
                {MasaStackConstant.PM,"Operator" },
                {MasaStackConstant.DCC,"Operator" },
                {MasaStackConstant.ALERT,"Operator" },
                {MasaStackConstant.MC,"Operator" },
                {MasaStackConstant.TSC,"Operator" },
                {MasaStackConstant.AUTH,"BasicAbility" },
                {MasaStackConstant.SCHEDULER,"BasicAbility" }
            };
            labelDictionary.TryGetValue(projectIdentity, out string? label);
            return label ?? "Other";
        }

        private static AddAppDto GenAppDto(KeyValuePair<string, System.Text.Json.Nodes.JsonNode?> keyValuePair)
        {
            var type = keyValuePair.Key.ToLower();
            AppTypes appType = AppTypes.UI;
            if (type == "web" || type == "sso")
            {
                appType = AppTypes.UI;
            }
            else if (type == "service")
            {
                appType = AppTypes.Service;

            }
            else if (type == "job" || type == "worker")
            {
                appType = AppTypes.Job;
            }

            var app = new AddAppDto
            {
                ServiceType = ServiceTypes.WebAPI,
                Type = appType,
                Identity = keyValuePair.Value?["id"]?.ToString() ?? "",
                Name = keyValuePair.Value?["name"]?.ToString() ?? "",
                Description = ""
            };

            return app;
        }
    }
}
