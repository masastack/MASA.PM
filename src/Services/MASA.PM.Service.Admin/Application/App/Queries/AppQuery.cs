// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.App.Queries
{
    public record AppQuery(bool IsHaveEnvironmentClusterInfo, int AppId) : Query<AppDto>
    {
        public override AppDto Result { get; set; } = new AppDto();
    }
}
