// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System.ComponentModel;

namespace MASA.PM.Contracts.Admin.Enum
{
    public enum AppTypes
    {
        [Description("Service")]
        Service = 1,

        [Description("UI")]
        UI,

        [Description("Job")]
        Job
    }
}
