namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("ProjectTypes")]
    [Index(nameof(Name), nameof(IsDeleted), Name = "IX_Name")]
    public class Label : BaseEntity<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "ProjectType name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "ProjectType name length range is [2-100]")]
        public string Name { get; set; } = "";
    }
}
