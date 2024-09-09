﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class ProjectDetailDto : BaseDto
    {
        public int Id { get; set; }

        public string Identity { get; set; } = "";

        public string LabelCode { get; set; } = "";

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public List<Guid> TeamIds { get; set; } = default!;

        public List<EnvironmentProjectTeamDto> EnvironmentProjectTeams { get; set; } = new();

        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();
    }
}
