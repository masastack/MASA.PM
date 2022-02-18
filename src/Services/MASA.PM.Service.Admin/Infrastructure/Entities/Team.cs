using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Teams")]
    public class Team : AuditAggregateRoot<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "Team name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Team name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("Description")]
        [Required(ErrorMessage = "Team description is required")]
        [StringLength(250, ErrorMessage = "Team description length must be less than 250")]
        public string Description { get; set; } = "";

        [Comment("AvatarPath")]
        [Required(ErrorMessage = "Team avatar path is required")]
        [StringLength(250, ErrorMessage = "Team avatar path length must be less than 250")]
        public string AvatarPath { get; set; } = "";
    }
}
