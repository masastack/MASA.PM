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
                    new AddEnvironmentDto
                    {
                        Name = "Development",
                        Description="开发环境",
                        Color = "#FF7D00"
                    },
                    new AddEnvironmentDto
                    {
                        Name = "Staging",
                        Description="模拟环境",
                        Color = "#37A7FF"
                    },
                    new AddEnvironmentDto
                    {
                        Name = "Production",
                        Description="生产环境",
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

        public static List<AddProjectAppDto> ProjectApps => new()
        {
            new AddProjectAppDto
            {
                Name = "Masa.Auth",
                Identity="masa-auth",
                LabelCode="BasicAbility",
                TeamId = Guid.Empty,
                Description = "auth",
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Auth.Web.Admin",
                        Identity = "masa-auth-web-admin",
                        Type = AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Auth.Service.Admin",
                        Identity = "masa-auth-service-admin",
                        Type = AppTypes.Service,
                        ServiceType = ServiceTypes.WebAPI
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Pm",
                Identity="masa-pm",
                LabelCode="BasicAbility",
                TeamId = Guid.Empty,
                Description = "pm",
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Pm.Web.Admin",
                        Identity = "masa-pm-web-admin",
                        Type = AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Pm.Service.Admin",
                        Identity = "masa-pm-service-admin",
                        Type = AppTypes.Service,
                        ServiceType = ServiceTypes.WebAPI
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Dcc",
                Identity="masa-dcc",
                LabelCode="BasicAbility",
                TeamId = Guid.Empty,
                Description = "dcc",
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Dcc.Web.Admin",
                        Identity = "masa-dcc-web-admin",
                        Type = AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Dcc.Service.Admin",
                        Identity = "masa-dcc-service-admin",
                        Type = AppTypes.Service,
                        ServiceType = ServiceTypes.WebAPI
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Mc",
                Identity="masa-mc",
                LabelCode="Other",
                TeamId = Guid.Empty,
                Description = "mc",
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Mc.Web.Admin",
                        Identity = "masa-mc-web-admin",
                        Type = AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Mc.Service.Admin",
                        Identity = "masa-mc-service-admin",
                        Type = AppTypes.Service,
                        ServiceType = ServiceTypes.WebAPI
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Scheduler",
                Identity="masa-scheduler",
                LabelCode="Other",
                TeamId = Guid.Empty,
                Description = "scheduler",
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Scheduler.Web.Admin",
                        Identity = "masa-scheduler-web-admin",
                        Type = AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Scheduler.Service.Server",
                        Identity = "masa-scheduler-service-server",
                        Type = AppTypes.Service,
                        ServiceType = ServiceTypes.WebAPI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Scheduler.Service.Worker",
                        Identity = "masa-scheduler-service-worker",
                        Type = AppTypes.Service,
                        ServiceType = ServiceTypes.WebAPI
                    }
                }
            },
        };

        public static async Task InitAsync(
            InitDto initDto,
            IMasaStackConfig masaStackConfig,
            IEnvironmentRepository environmentRepository,
            IClusterRepository clusterRepository,
            IProjectRepository projectRepository,
            IAppRepository appRepository)
        {
            //environment
            var envs = initDto.Environments.Select(e => new Infrastructure.Entities.Environment
            {
                Name = e.Name,
                Description = e.Description,
                Color = e.Color
            });
            var envEntitis = new List<Infrastructure.Entities.Environment>();
            foreach (var env in envs)
            {
                var newEnv = await environmentRepository.AddAsync(env);
                envEntitis.Add(newEnv);
            }

            //cluster
            var cluster = await clusterRepository.AddAsync(new Infrastructure.Entities.Cluster
            {
                Name = initDto.ClusterName,
                Description = initDto.ClusterDescription
            });
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
            var projects = SeedData.GetProjectApps(masaStackConfig);
            var projectIds = new List<int>();
            var envClusterProject = new List<EnvironmentClusterProject>();
            var appGroups = new List<(int ProjectId, string ProjectDescription, int AppId, string Description)>();
            var envClusterProjectApps = new List<EnvironmentClusterProjectApp>();
            foreach (var project in projects)
            {
                var newProject = await projectRepository.AddAsync(project.Adapt<Infrastructure.Entities.Project>());
                projectIds.Add(newProject.Id);

                foreach (var app in project.Apps)
                {
                    var newApp = await appRepository.AddAsync(app.Adapt<Infrastructure.Entities.App>());
                    appGroups.Add((newProject.Id, newProject.Description, newApp.Id, newApp.Description));
                }

                foreach (var envCluster in envClusetr)
                {
                    var newEnvironmentClusterProject = await projectRepository.AddEnvironmentClusterProjectAsync(new EnvironmentClusterProject
                    {
                        EnvironmentClusterId = envCluster.Id,
                        ProjectId = newProject.Id
                    });

                    foreach (var app in appGroups)
                    {
                        if (app.ProjectId == newProject.Id)
                        {
                            var envName = envEntitis.Find(env => env.Id == envCluster.EnvironmentId)?.Name.ToLower() ?? "develop";
                            envClusterProjectApps.Add(new EnvironmentClusterProjectApp
                            {
                                EnvironmentClusterProjectId = newEnvironmentClusterProject.Id,
                                AppId = app.AppId,
                                AppURL = $"https://{app.ProjectDescription}-{envName}.masastack.com"
                            });
                        }
                    }
                }
            }

            await appRepository.AddEnvironmentClusterProjectAppsAsync(envClusterProjectApps);
        }

        public static List<AddProjectAppDto> GetProjectApps(IMasaStackConfig masaStackConfig)
        {
            var allServer = masaStackConfig.GetAllServer();
            List<AddProjectAppDto> projectApps = new List<AddProjectAppDto>();
            var teamId = Guid.Empty;
            var labelCode = "Other";
            foreach (var server in allServer)
            {
                var project = server.Key;

                AddProjectAppDto projectApp = new AddProjectAppDto
                {
                    Name = project,
                    Identity = project,
                    LabelCode = labelCode,
                    TeamId = teamId,
                    Description = project
                };

                var apps = server.Value;
                foreach (var item in apps.ToList())
                {
                    projectApp.Apps.Add(GenAppDto(item));
                }

                projectApps.Add(projectApp);
            }

            return projectApps;
        }

        private static AddAppDto GenAppDto(KeyValuePair<string, System.Text.Json.Nodes.JsonNode?> keyValuePair)
        {
            var type = keyValuePair.Key.ToLower();
            var appIdentity = keyValuePair.Value!.ToString();
            var name = keyValuePair.Value.ToString().ToName();
            AppTypes appType = AppTypes.UI;
            if (type == "ui" || type == "sso")
            {
                appType = AppTypes.UI;
            }
            else if (type == "server")
            {
                appType = AppTypes.Service;

            }
            else if (type == "job" || type == "work")
            {
                appType = AppTypes.Job;
            }

            var app = new AddAppDto
            {
                ServiceType = ServiceTypes.WebAPI,
                Type = appType,
                Identity = appIdentity,
                Name = name,
            };

            return app;
        }
    }
}
