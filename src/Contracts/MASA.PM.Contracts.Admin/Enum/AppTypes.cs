using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
