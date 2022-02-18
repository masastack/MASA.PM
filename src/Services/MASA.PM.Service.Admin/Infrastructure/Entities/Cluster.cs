
namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Clusters")]
    public class Cluster : AuditAggregateRoot<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "Cluster name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Cluster name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("Name")]
        [Required(ErrorMessage = "Cluster description is required")]
        [StringLength(250, ErrorMessage = "Cluster description length must be less than 250")]
        public string Description { get; set; } = "";
    }
}
