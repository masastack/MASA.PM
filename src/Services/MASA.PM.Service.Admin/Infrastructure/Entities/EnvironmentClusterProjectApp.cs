using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("EnvironmentClusterProjectApps")]
    [Index(nameof(EnvironmentClusterProjectId), Name = "IX_EnvironmentClusterId")]
    public class EnvironmentClusterProjectApp : Entity<int>
    {
        [Comment("Environment cluster project Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Environment cluster project is required")]
        public int EnvironmentClusterProjectId { get; set; }

        [Comment("App Id")]
        [Range(1, int.MaxValue, ErrorMessage = "App is required")]
        public int AppId { get; set; }
    }
}
