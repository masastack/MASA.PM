// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record DeleteClusterCommand(int EnvClusterId) : Command
    {
    }
}
