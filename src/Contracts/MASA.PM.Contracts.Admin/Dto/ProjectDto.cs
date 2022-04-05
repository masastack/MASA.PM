using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Identity { get; set; } = "";

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public int LableId { get; set; }

        public string LableName { get; set; } = "";

        public Guid Modifier { get; set; }

        public DateTime ModificationTime { get; set; }
    }
}
