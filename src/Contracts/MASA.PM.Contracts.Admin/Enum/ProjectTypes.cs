﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Enum
{
    public enum ProjectTypes
    {
        [Description("Basic Ability")]
        BasicAbility = 1,

        [Description("Ops Ability")]
        OpsAbility,

        [Description("Data Factory")]
        DataFactory
    }
}
