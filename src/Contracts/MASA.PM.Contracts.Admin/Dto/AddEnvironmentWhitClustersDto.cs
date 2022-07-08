// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddEnvironmentWhitClustersDto : AddEnvironmentDto
    {
        public List<int> ClusterIds { get; set; } = new();

        public AddEnvironmentWhitClustersDto()
        {

        }

        public AddEnvironmentWhitClustersDto(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
