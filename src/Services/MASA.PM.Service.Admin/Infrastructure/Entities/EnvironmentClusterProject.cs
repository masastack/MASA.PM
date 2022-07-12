// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("EnvironmentClusterProjects")]
    [Index(nameof(EnvironmentClusterId), Name = "IX_EnvironmentClusterId")]
    public class EnvironmentClusterProject : Entity<int>
    {
        [Comment("Environment cluster Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Environment cluster is required")]
        public int EnvironmentClusterId { get; set; }

        [Comment("System Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Project is required")]
        public int ProjectId { get; set; }
    }
}
