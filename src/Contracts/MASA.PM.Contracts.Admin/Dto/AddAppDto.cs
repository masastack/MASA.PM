// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddAppDto
    {
        private string _name = default!;
        private string _swaggerUrl = "";

        //[RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "App name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "App name length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim() ?? "";
        }

        [MinCount(1, ErrorMessage = "15s4d5sds")]
        public List<int> EnvironmentClusterIds { get; set; } = new();

        public int ProjectId { get; set; }

        [NonDefault]
        public AppTypes Type { get; set; }

        public ServiceTypes ServiceType { get; set; }

        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "Please enter [English、and - _ symbols] ")]
        [Required(ErrorMessage = "Identity is required ")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Identity length range is [2-50]")]
        public string Identity { get; set; } = "";

        public string Url { get; set; } = "";

        public string SwaggerUrl
        {
            get => _swaggerUrl;
            set => _swaggerUrl = value?.Trim() ?? "";
        }

        [StringLength(255, ErrorMessage = "App description length must be less than 255")]
        public string Description { get; set; } = "";
    }
}
