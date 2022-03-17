using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class RemoveAppDto
    {
        public int AppId { get; set; }

        public int ProjectId { get; set; }
    }
}
