using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class AddClusterModel
    {
        private string _name = default!;

        [Required(ErrorMessage = "Cluster name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Cluster name length range is [2-100]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [StringLength(250, ErrorMessage = "Cluster description length must be less than 250")]
        public string Description { get; set; } = "";
    }
}
