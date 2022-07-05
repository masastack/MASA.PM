﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class ClusterDetailDto : BaseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = "";

        public List<int> EnvironmentIds { get; set; } = new();
    }
}
