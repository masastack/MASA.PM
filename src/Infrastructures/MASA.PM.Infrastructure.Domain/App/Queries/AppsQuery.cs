// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.App.Queries;

public record AppsQuery(List<int> ProjectIds) : Query<List<AppDto>>
{
    public override List<AppDto> Result { get; set; } = new();
}
