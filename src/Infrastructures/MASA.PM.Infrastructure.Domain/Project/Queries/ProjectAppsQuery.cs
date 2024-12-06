// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Project.Queries;

public record ProjectAppsQuery(string EnvName) : Query<List<ProjectModel>>
{
    public override List<ProjectModel> Result { get; set; } = new();
}
