using MASA.PM.Contracts.Base.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.ViewModel
{
    public class AppViewModel: AuditViewModel
    {
        public int ProjectId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Identity { get; set; } = "";

        public string Description { get; set; } = ""; 

        public AppTypes Type { get; set; }

        public ServiceTypes ServiceType { get; set; }

        public string Url { get; set; } = "";

        public string SwaggerUrl { get; set; } = "";

        public List<AppEnvironmentClusterViewModel> EnvironmentClusters { get; set; } = new();
    }
}
