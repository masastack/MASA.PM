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
                                AppURL = masaStackConfig.GetUIDomain("http", project.Name.ToLower(), app.Description)
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
            var allUI = masaStackConfig.GetAllUI();
            var allService = allServer.Union(allUI).ToList().GroupBy(s => s.Key)
                .Select(g => new { g.Key, Value = g.SelectMany(a => a.Value).ToList() }).ToList();

            List<AddProjectAppDto> projectApps = new List<AddProjectAppDto>();
            var teamId = Guid.Empty;
            foreach (var service in allService)
            {
                var project = service.Key;

                AddProjectAppDto projectApp = new AddProjectAppDto
                {
                    Name = project.ToUpper(),
                    Identity = project,
                    LabelCode = GetLabel(project),
                    TeamId = teamId,
                    Description = project
                };

                var apps = service.Value;
                foreach (var app in apps)
                {
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
                Description = type
            };

            return app;
        }
    }
}
