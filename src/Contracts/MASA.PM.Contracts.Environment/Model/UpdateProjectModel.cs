using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class UpdateProjectModel : AddProjectModel
    {
        public int ProjectId { get; set; }

        public Guid ActionUserId { get; set; }
    }
}
