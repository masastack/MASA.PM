// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddEnvironmentWhitClustersDto : AddEnvironmentDto
    {
        [MinCount(1, ErrorMessage = "Related cluster is required")]
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
