using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class EnvironmentClusterDto
    {
        public int Id { get; set; }

        public string EnvironmentName { get; set; } = "";

        public string ClusterName { get; set; } = "";
    }
}
