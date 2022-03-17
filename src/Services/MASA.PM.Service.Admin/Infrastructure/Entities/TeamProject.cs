using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("TeamProjects")]
    public class TeamProject
    {
        [Comment("Team Id")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId is required")]
        public int TeamId { get; set; }

        [Comment("Project Id")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId is required")]
        public int ProjectId { get; set; }

        [Comment("Authorization team Id")]
        public int AuthorizationTeamId { get; set; }

        [Comment("Authorization accept")]
        public bool AuthorizationAccept { get; set; }

        public bool IsDeleted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [Required]
        public Guid Creator { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModificationTime { get; set; } = DateTime.Now;

        [Required]
        public Guid Modifier { get; set; }
    }
}
