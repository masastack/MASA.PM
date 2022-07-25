// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddRelationAppDto
    {
        public int AppId { get; set; }

        [MinCount(1)]
        public List<int> EnvironmentClusterIds { get; set; } = new();

        public int ProjectId { get; set; }
    }
}
