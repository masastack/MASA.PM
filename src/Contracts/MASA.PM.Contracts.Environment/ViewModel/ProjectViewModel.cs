using MASA.PM.Contracts.Base.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.ViewModel
{
    public class ProjectViewModel : AuditViewModel
    {
        public int Id { get; set; }

        public string Identity { get; set; } = "";

        public ProjectTypes Type { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public Guid TeamId { get; set; }

        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();
    }
}
