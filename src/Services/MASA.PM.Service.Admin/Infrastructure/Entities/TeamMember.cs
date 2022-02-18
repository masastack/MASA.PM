using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("TeamMembers")]
    public class TeamMember
    {
        [Comment("Team Id")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId is required")]
        public int TeamId { get; set; }

        [Comment("User Id")]
        [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; }

        [Comment("Is administrator")]
        public bool IsAdministrator { get; set; }
    }
}
