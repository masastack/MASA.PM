// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Migrations
{
    public static class SeedData
    {
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

        public static async Task InitDCCDataAsync(this WebApplicationBuilder builder)
        {
            var services = builder.Services.BuildServiceProvider();
            var configurationApiManage = services.GetRequiredService<IConfigurationApiManage>();
            var env = services.GetRequiredService<IWebHostEnvironment>();
            var environment = builder.Environment.EnvironmentName;
            var filePath = CombineFilePath(env.ContentRootPath, "appsettings.json", environment);
            var content = File.ReadAllText(filePath);
            await configurationApiManage.AddAsync(environment, "default", "masa-pm-service-admin", new Dictionary<string, string>
            {
                {"Appsettings",content }
            });
        }

        private static string CombineFilePath(string contentRootPath, string fileName, string environment)
        {
            string extension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var environmentFileName = $"{fileNameWithoutExtension}.{environment}{extension}";
            var environmentFilePath = Path.Combine(contentRootPath, "Config", environmentFileName);
            if (File.Exists(environmentFilePath))
            {
                return environmentFilePath;
            }
            return Path.Combine(contentRootPath, "Config", fileName);
        }
    }
}
