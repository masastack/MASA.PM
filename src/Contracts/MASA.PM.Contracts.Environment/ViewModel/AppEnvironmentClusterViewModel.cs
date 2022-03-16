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

        public int ProjectId { get; set; }

        public EnvironmentClusterViewModel EnvironmentCluster { get; set; } = new();
    }
}
