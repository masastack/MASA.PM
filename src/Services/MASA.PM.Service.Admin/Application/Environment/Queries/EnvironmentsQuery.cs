// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Environment.Queries
{
    public record EnvironmentsQuery : Query<List<EnvironmentDto>>
    {
        public override List<EnvironmentDto> Result { get; set; } = new();
    }
}
