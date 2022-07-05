// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class InitDto
    {
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Cluster name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Cluster name length range is [2-50]")]
        public string ClusterName { get; set; } = "";

        [StringLength(250, ErrorMessage = "Cluster description length must be less than 250")]
        public string ClusterDescription { get; set; } = "";

        public List<AddEnvironmentDto> Environments { get; set; } = new();
    }
}
