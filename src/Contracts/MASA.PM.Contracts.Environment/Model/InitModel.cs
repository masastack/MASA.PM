using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class InitModel
    {
        public string ClusterName { get; set; } = "";

        public string ClusterDescription { get; set; } = "";

        public List<AddEnvironmentModel> Environments { get; set; } = new();
    }
}
