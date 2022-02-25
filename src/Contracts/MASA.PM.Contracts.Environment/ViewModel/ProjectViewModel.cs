using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.ViewModel
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public int AppCount { get; set; }

        public Guid Modifier { get; set; }

        public DateTime ModificationTime { get; set; }
    }
}
