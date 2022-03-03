using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class AddProjectModel
    {
        private string _name = default!;

        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Project name length range is [2-100]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [MinCount(1, ErrorMessage = "EnvironmentClusterIds is required")]
        public List<int> EnvironmentClusterIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "TeamId is required")]
        public Guid TeamId { get; set; }

        [StringLength(250, ErrorMessage = "Project description length must be less than 250")]
        public string Description { get; set; } = "";
    }
}
