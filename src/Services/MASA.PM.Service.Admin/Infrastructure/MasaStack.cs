// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure;

public static class MasaStack
{
    public static Dictionary<string, string> MasaStackIdNamePairs => new Dictionary<string, string>
    {
        { MasaStackProject.PM.Name, "MASA PM" },
        { MasaStackProject.DCC.Name, "MASA DCC" },
        { MasaStackProject.Auth.Name,"MASA Auth" },
        { MasaStackProject.MC.Name, "MASA MC" },
        { MasaStackProject.Alert.Name, "MASA Alert" },
        { MasaStackProject.Scheduler.Name, "MASA Scheduler" },
        { MasaStackProject.TSC.Name, "MASA TSC" }
    };

    public static Dictionary<string, string> MasaStackIdLabelPairs => new Dictionary<string, string>
    {
        { MasaStackProject.PM.Name, "Operator" },
        { MasaStackProject.DCC.Name, "Operator" },
        { MasaStackProject.Alert.Name, "Operator" },
        { MasaStackProject.MC.Name, "Operator" },
        { MasaStackProject.TSC.Name, "Operator" },
        { MasaStackProject.Auth.Name, "BasicAbility" },
        { MasaStackProject.Scheduler.Name, "BasicAbility" }
    };
}
