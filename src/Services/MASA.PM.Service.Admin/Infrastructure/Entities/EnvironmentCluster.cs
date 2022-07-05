// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("EnvironmentClusters")]
    [Index(nameof(EnvironmentId), Name = "IX_EnvironmentId")]
    [Index(nameof(ClusterId), Name = "IX_ClusterId")]
    public class EnvironmentCluster : Entity<int>
    {
        [Comment("Environment Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Environment is required")]
        public int EnvironmentId { get; set; }

        [Comment("Cluster Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Cluster is required")]
        public int ClusterId { get; set; }
    }
}
