﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddClusterWhitEnvironmentsDto
    {
        private string _name = default!;

        [Display(Name = "Cluster name")]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim() ?? "";
        }

        [MinCount(1, ErrorMessage = "Related environment is required")]
        public List<int> EnvironmentIds { get; set; } = new List<int>();

        [StringLength(255, ErrorMessage = "Cluster description length must be less than 255")]
        public string Description { get; set; } = "";
    }
}
