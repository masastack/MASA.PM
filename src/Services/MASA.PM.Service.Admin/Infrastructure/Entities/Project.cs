
namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Projects")]
    public class Project : AuditAggregateRoot<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "System name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "System name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("TeamId")]
        [Range(1, int.MaxValue, ErrorMessage = "TeamId is required")]
        [Column(TypeName = "tinyint")]
        public int TeamId { get; set; }

        [Comment("Description")]
        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250, MinimumLength = 0, ErrorMessage = "Description length range is [0-250]")]
        public string Description { get; set; } = "";
    }
}
