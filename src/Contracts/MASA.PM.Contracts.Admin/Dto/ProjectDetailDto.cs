// Copyright (c) MASA Stack All rights reserved.
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

        public Guid TeamId { get; set; }

        public string TeamName { get; set; } = "";

        public string TeamAvatar { get; set; } = "";

        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();
    }
}
