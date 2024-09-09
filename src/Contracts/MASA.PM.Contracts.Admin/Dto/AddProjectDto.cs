// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddProjectDto
    {
        private string _name = default!;

        //[RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Project name length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim() ?? "";
        }

        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Project identity is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "App identity length range is [2-50]")]
        public string Identity { get; set; } = "";

        [Required]
        public string LabelCode { get; set; } = "";

        [MinCount(1, ErrorMessage = "EnvironmentClusterIds is required")]
        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "TeamIds is required")]
        public List<Guid> TeamIds { get; set; } = default!;

        [StringLength(255, ErrorMessage = "Project description length must be less than 255")]
        public string Description { get; set; } = "";

        [Required]
        public string EnvironmentName { get; set; } = "";
    }
}
