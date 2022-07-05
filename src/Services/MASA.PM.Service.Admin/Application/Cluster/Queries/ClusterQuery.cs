// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record ClusterQuery : Query<ClusterDetailDto>
    {
        public override ClusterDetailDto Result { get; set; } = new ClusterDetailDto();

        public int ClusterId { get; set; }
    }
}
