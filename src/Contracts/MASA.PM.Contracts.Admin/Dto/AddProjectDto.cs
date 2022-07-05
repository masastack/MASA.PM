// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using MASA.PM.Contracts.Admin.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddProjectDto
    {
        private string _name = default!;

        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Project name length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim() ?? "";
        }

        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Project identity is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "App identity length range is [2-50]")]
        public string Identity { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Project label is required")]
        public int LabelId { get; set; }

        [MinCount(1, ErrorMessage = "EnvironmentClusterIds is required")]
        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "TeamId is required")]
        public Guid TeamId { get; set; }

        [StringLength(255, ErrorMessage = "Project description length must be less than 255")]
        public string Description { get; set; } = "";
    }
}
