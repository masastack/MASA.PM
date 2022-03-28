using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddClusterWhitEnvironmentsDto
    {
        private string _name = default!;

        [Display(Name = "Cluster name")]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [Required(ErrorMessage = "Related environment is required")]
        public List<int> EnvironmentIds { get; set; } = default!;

        [StringLength(250, ErrorMessage = "Cluster description length must be less than 250")]
        public string Description { get; set; } = "";
    }
}
