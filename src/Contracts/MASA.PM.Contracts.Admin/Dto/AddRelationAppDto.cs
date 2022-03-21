using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddRelationAppDto
    {
        public int AppId { get; set; }

        public List<int> EnvironmentClusterIds { get; set; } = new();

        public int ProjectId { get; set; }
    }
}
