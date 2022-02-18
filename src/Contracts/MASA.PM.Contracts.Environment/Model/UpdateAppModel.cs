using MASA.PM.Contracts.Base.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class UpdateAppModel
    {
        [Required]
        public int Id { get; set; }

        private string _name = default!;

        [Required(ErrorMessage = "App name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "App name length range is [2-100]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        public string Url { get; set; } = "";

        public string SwaggerUrl { get; set; } = "";

        [StringLength(250, ErrorMessage = "App description length must be less than 250")]
        public string Description { get; set; } = "";

        public Guid ActionUserId { get; set; }
    }
}
