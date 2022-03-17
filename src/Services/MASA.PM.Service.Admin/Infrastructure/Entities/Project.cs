using MASA.PM.Contracts.Admin.Enum;

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Projects")]
    public class Project : AuditAggregateRoot<int, Guid>
    {
        [Comment("Identity")]
        [Required(ErrorMessage = "Project identity is required")]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "App identity length range is [2-100]")]
        public string Identity { get; set; } = "";

        [Comment("Type")]
        [Range(1, int.MaxValue, ErrorMessage = "Project type is required")]
        [Column(TypeName = "tinyint")]
        public ProjectTypes Type { get; set; }

        [Comment("Name")]
        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "System name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("TeamId")]
        [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "TeamId is required")]
        public Guid TeamId { get; set; }

        [Comment("Description")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Description length range is [0-255]")]
        public string Description { get; set; } = "";
    }
}
