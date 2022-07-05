// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record AddEnvironmentCommand(AddEnvironmentWhitClustersDto EnvironmentWhitClusterModel) : Command
    {
        public EnvironmentDto Result { get; set; } = default!;
    }
}
