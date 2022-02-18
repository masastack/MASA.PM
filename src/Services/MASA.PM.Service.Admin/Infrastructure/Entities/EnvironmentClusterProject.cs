using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("EnvironmentClusterProjects")]
    [Index(nameof(EnvironmentClusterId), nameof(IsDeleted), Name = "IX_EnvironmentClusterId_IsDeleted")]
    public class EnvironmentClusterProject : AuditAggregateRoot<int, Guid>
    {
        [Comment("Environment cluster Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Environment cluster is required")]
        public int EnvironmentClusterId { get; set; }

        [Comment("System Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Project is required")]
        public int ProjectId { get; set; }
    }
}
