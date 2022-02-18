using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.ViewModel
{
    public class AppEnvironmentClusterViewModel
    {
        public int AppId { get; set; }

        public int EnvironmentClusterId { get; set; }

        public string EnvironmentClusterName { get; set; } = "";
    }
}
