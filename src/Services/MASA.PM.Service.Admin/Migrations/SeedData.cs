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
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Auth.Web.Admin",
                        Identity = "masa-auth-web-admin",
                        Type = Contracts.Admin.Enum.AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Auth.Service.Admin",
                        Identity = "masa-auth-service-admin",
                        Type = Contracts.Admin.Enum.AppTypes.Service,
                        ServiceType = Contracts.Admin.Enum.ServiceTypes.WebApi
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Pm",
                Identity="masa-pm",
                LabelCode="BasicAbility",
                TeamId = Guid.Empty,
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Pm.Web.Admin",
                        Identity = "masa-pm-web-admin",
                        Type = Contracts.Admin.Enum.AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Pm.Service.Admin",
                        Identity = "masa-pm-service-admin",
                        Type = Contracts.Admin.Enum.AppTypes.Service,
                        ServiceType = Contracts.Admin.Enum.ServiceTypes.WebApi
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Dcc",
                Identity="masa-dcc",
                LabelCode="BasicAbility",
                TeamId = Guid.Empty,
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Dcc.Web.Admin",
                        Identity = "masa-dcc-web-admin",
                        Type = Contracts.Admin.Enum.AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Dcc.Service.Admin",
                        Identity = "masa-dcc-service-admin",
                        Type = Contracts.Admin.Enum.AppTypes.Service,
                        ServiceType = Contracts.Admin.Enum.ServiceTypes.WebApi
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Mc",
                Identity="masa-mc",
                LabelCode="Other",
                TeamId = Guid.Empty,
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Mc.Web.Admin",
                        Identity = "masa-mc-web-admin",
                        Type = Contracts.Admin.Enum.AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Mc.Service.Admin",
                        Identity = "masa-mc-service-admin",
                        Type = Contracts.Admin.Enum.AppTypes.Service,
                        ServiceType = Contracts.Admin.Enum.ServiceTypes.WebApi
                    }
                }
            },
            new AddProjectAppDto
            {
                Name = "Masa.Scheduler",
                Identity="masa-scheduler",
                LabelCode="Other",
                TeamId = Guid.Empty,
                Apps = new List<AddAppDto>
                {
                    new AddAppDto
                    {
                        Name = "Masa.Scheduler.Web.Admin",
                        Identity = "masa-scheduler-web-admin",
                        Type = Contracts.Admin.Enum.AppTypes.UI
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Scheduler.Service.Server",
                        Identity = "masa-scheduler-service-server",
                        Type = Contracts.Admin.Enum.AppTypes.Service,
                        ServiceType = Contracts.Admin.Enum.ServiceTypes.WebApi
                    },
                    new AddAppDto
                    {
                        Name = "Masa.Scheduler.Service.Worker",
                        Identity = "masa-scheduler-service-worker",
                        Type = Contracts.Admin.Enum.AppTypes.Service,
                        ServiceType = Contracts.Admin.Enum.ServiceTypes.WebApi
                    }
                }
            },
        };
    }
}
