namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record ClusterQuery : Query<ClusterDetailDto>
    {
        public override ClusterDetailDto Result { get; set; } = new ClusterDetailDto();

        public int ClusterId { get; set; }
    }
}
