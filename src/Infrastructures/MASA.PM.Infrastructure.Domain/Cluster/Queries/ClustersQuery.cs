// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Cluster.Queries;

public record ClustersQuery : Query<List<ClusterDto>>
{
    public override List<ClusterDto> Result { get; set; } = new();
}
