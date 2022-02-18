
namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("EnvironmentClusters")]
    [Index(nameof(EnvironmentId), nameof(IsDeleted), Name = "IX_EnvironmentId_IsDeleted")]
    public class EnvironmentCluster : AuditAggregateRoot<int, Guid>
    {
        [Comment("Environment Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Environment is required")]
        public int EnvironmentId { get; set; }

        [Comment("Cluster Id")]
        [Range(1, int.MaxValue, ErrorMessage = "Cluster is required")]
        public int ClusterId { get; set; }

        [Comment("Is default")]
        public bool IsDefault { get; set; }
    }
}
