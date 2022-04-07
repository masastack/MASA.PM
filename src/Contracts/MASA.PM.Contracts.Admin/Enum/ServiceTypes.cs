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
