using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class AddTeam
    {
        [Required(ErrorMessage = "Team name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Team name length range is [2-100]")]
        public string Name { get; set; } = default!;

        [StringLength(250, MinimumLength = 0, ErrorMessage = "Team description length must be less than 250")]
        public string Description { get; set; } = "";

        [StringLength(250, MinimumLength = 0, ErrorMessage = "Team avatar path length must be less than 250")]
        public string AvatarPath { get; set; } = "";
    }
}
