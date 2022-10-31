// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectByClusterIdQuery(int ClusterId) : Query<bool>
    {
        public override bool Result { get; set; }
    }
}
