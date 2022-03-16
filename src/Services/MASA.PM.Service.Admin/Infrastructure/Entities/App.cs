using MASA.PM.Contracts.Base.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Apps")]
    public class App : AuditAggregateRoot<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "App name is required")]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "App name length range is [2-100]")]
        public string Name { get; set; } = "";

        /// <summary>
        /// App Id
        /// </summary>
        [Comment("Identity")]
        [Required(ErrorMessage = "App identity is required")]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "App identity length range is [2-100]")]
        public string Identity { get; set; } = "";

        [Comment("Type")]
        [Range(1, int.MaxValue, ErrorMessage = "App type is required")]
        [Column(TypeName = "tinyint")]
        //public AppTypes Type { get; set; }
        public AppTypes Type { get; set; }

        [Comment("ServiceType")]
        [Range(1, int.MaxValue, ErrorMessage = "App service type is required")]
        [Column(TypeName = "tinyint")]
        //public ServiceTypes ServiceType { get; set; }
        public ServiceTypes ServiceType { get; set; }

        [Comment("Url")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Url length range is [0-255]")]
        public string Url { get; set; } = "";

        [Comment("SwaggerUrl")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Url length range is [0-255]")]
        public string SwaggerUrl { get; set; } = "";

        [Comment("Description")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Description length range is [0-255]")]
        public string Description { get; set; } = "";
    }
}
