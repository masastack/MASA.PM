using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddAppDto
    {
        private string _name = default!;

        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "App name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "App name length range is [2-50]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }
        
        public List<int> EnvironmentClusterIds { get; set; } = new();

        public int ProjectId { get; set; }

        [NonDefault]
        public AppTypes Type { get; set; }

        [NonDefault]
        public ServiceTypes ServiceType { get; set; }

        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "Identity is required ")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Identity length range is [2-50]")]
        public string Identity { get; set; } = "";

        public string Url { get; set; } = "";

        public string SwaggerUrl { get; set; } = "";

        [StringLength(255, ErrorMessage = "App description length must be less than 255")]
        public string Description { get; set; } = "";
    }
}
