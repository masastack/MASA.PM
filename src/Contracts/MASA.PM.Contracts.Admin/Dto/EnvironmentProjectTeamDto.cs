// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class EnvironmentProjectTeamDto
    {
        public int ProjectId { get; set; }

        public string EnvironmentName { get; set; } = "";

        public Guid TeamId { get; set; }
    }
}
