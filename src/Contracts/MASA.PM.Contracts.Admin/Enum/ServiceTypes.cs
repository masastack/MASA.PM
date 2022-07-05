// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Enum
{
    public enum ServiceTypes
    {
        [Description("Dapr")]
        Dapr = 1,

        [Description("Web Api")]
        WebApi
    }
}
