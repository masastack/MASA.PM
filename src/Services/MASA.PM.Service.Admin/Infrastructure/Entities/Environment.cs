
namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Environments")]
    public class Environment : BaseEntity<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "Environment name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Environment name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("Description")]
        [Required(ErrorMessage = "Environment description is required")]
        [StringLength(255, ErrorMessage = "Environment description length must be less than 255")]
        public string Description { get; set; } = "";

        [Comment("Color")]
        [Required(ErrorMessage = "Environment color is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Environment color length range is [2-10]")]
        public string Color { get; set; } = "";
    }
}