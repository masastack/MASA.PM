using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class AddEnvironmentWhitClustersModel : AddEnvironmentModel
    {
        [Required(ErrorMessage = "Related cluster is required")]
        public List<int> ClusterIds { get; set; } = default!;

        public AddEnvironmentWhitClustersModel()
        {

        }

        public AddEnvironmentWhitClustersModel(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
