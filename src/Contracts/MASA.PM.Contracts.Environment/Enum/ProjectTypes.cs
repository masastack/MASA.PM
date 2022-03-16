using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Enum
{
    public enum ProjectTypes
    {
        [Description("BaseAbility")]
        BaseAbility = 1,

        [Description("OpsAbility")]
        OpsAbility,

        [Description("DataFactory")]
        DataFactory
    }
}
