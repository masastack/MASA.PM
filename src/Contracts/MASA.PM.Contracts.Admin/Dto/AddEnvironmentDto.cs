using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class AddEnvironmentDto
    {
        private string _name = default!;

        [Display(Name = "Environment name")]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_-]+$", ErrorMessage = "Please enter [Chinese, English、and - _ symbols] ")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} length range is [2-100]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [StringLength(250, ErrorMessage = "Environment description length must be less than 250")]
        public string Description { get; set; } = "";

        public AddEnvironmentDto()
        {

        }

        public AddEnvironmentDto(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
