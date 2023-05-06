// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure;

public static class MasaStack
{
    public static Dictionary<string, string> MasaStackIdNamePairs => new Dictionary<string, string>
    {
        { MasaStackConstant.PM, "MASA PM" },
        { MasaStackConstant.DCC, "MASA DCC" },
        { MasaStackConstant.AUTH,"MASA Auth" },
        { MasaStackConstant.MC, "MASA MC" },
        { MasaStackConstant.ALERT, "MASA Alert" },
        { MasaStackConstant.SCHEDULER, "MASA Scheduler" },
        { MasaStackConstant.TSC, "MASA TSC" }
    };

    public static Dictionary<string, string> MasaStackIdLabelPairs => new Dictionary<string, string>
    {
        { MasaStackConstant.PM, "Operator" },
        { MasaStackConstant.DCC, "Operator" },
        { MasaStackConstant.ALERT, "Operator" },
        { MasaStackConstant.MC, "Operator" },
        { MasaStackConstant.TSC, "Operator" },
        { MasaStackConstant.AUTH, "BasicAbility" },
        { MasaStackConstant.SCHEDULER, "BasicAbility" }
    };
}
