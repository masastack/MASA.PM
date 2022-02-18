
namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Environments")]
    public class Environment : AuditAggregateRoot<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "Environment name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Environment name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("Description")]
        [Required(ErrorMessage = "Environment description is required")]
        [StringLength(250, ErrorMessage = "Environment description length must be less than 250")]
        public string Description { get; set; } = "";
    }
}