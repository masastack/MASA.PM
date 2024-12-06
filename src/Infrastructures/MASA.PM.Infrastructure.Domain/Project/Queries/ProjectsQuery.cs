// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Project.Queries;

public record ProjectsQuery(int? EnvironmentClusterId, List<Guid>? TeamIds, string? Environment = null) : Query<List<ProjectDto>>
{
    public override List<ProjectDto> Result { get; set; } = new List<ProjectDto>();
}
