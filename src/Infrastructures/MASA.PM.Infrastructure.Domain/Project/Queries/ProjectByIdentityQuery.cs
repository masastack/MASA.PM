// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Project.Queries;

public record ProjectByIdentityQuery(string identity) : Query<ProjectDetailDto>
{
    public override ProjectDetailDto Result { get; set; } = new ProjectDetailDto();
}
