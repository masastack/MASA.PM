using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class UpdateEnvironmentDto: AddEnvironmentWhitClustersDto
    {
        public int EnvironmentId { get; set; }
    }
}
