using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class ClusterDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public int EnvironmentClusterId { get; set; }
    }
}
