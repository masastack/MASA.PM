using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddClusterDto
    {
        private string _name = default!;

        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Cluster name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Cluster name length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [StringLength(255, ErrorMessage = "Cluster description length must be less than 255")]
        public string Description { get; set; } = "";
    }
}
