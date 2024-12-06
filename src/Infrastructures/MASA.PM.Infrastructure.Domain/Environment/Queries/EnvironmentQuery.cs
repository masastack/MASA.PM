// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Environment.Queries;

public record EnvironmentQuery : Query<EnvironmentDetailDto>
{
    public int EnvironmentId { get; set; }

    public override EnvironmentDetailDto Result { get; set; } = default!;
}
