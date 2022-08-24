// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class EnvironmentDetailDto : BaseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = "";

        public string Color { get; set; } = "";

        public List<int> ClusterIds { get; set; } = default!;
    }
}
