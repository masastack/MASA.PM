using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class ProjectDetailDto : BaseDto
    {
        public int Id { get; set; }

        public string Identity { get; set; } = "";

        public int TypeId { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public Guid TeamId { get; set; }

        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();
    }
}
