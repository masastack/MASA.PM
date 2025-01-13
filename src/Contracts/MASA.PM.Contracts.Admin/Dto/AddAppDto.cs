// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddAppDto
    {
        private string _name = default!;

        [Required(ErrorMessage = "App name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "App name length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim() ?? "";
        }

        [MinCount(1, ErrorMessage = "Environment cluster is required")]
        public List<EnvironmentClusterInfo> EnvironmentClusterInfos { get; set; } = new();

        public int ProjectId { get; set; }

        [NonDefault]
        public AppTypes Type { get; set; }

        public ServiceTypes ServiceType { get; set; }

        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "Please enter [English、and - _ symbols] ")]
        [Required(ErrorMessage = "Identity is required ")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Identity length range is [2-50]")]
        public string Identity { get; set; } = "";

        [StringLength(255, ErrorMessage = "App description length must be less than 255")]
        public string Description { get; set; } = "";

        public List<Guid> ResponsibilityUsers { get; set; } = new();
    }

    public record EnvironmentClusterInfo
    {
        public int EnvironmentClusterId { get; set; }

        public string Url { get; set; } = "";

        public string SwaggerUrl { get; set; } = "";

        public EnvironmentClusterInfo()
        {
        }

        public EnvironmentClusterInfo(int environmentClusterId)
        {
            EnvironmentClusterId = environmentClusterId;
        }

        public EnvironmentClusterInfo(int environmentClusterId, string url, string swaggerUrl) : this(environmentClusterId)
        {
            Url = url;
            SwaggerUrl = swaggerUrl;
        }
    }
}
