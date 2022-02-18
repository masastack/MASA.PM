namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record ClusterQuery : Query<ClusterViewModel>
    {
        public override ClusterViewModel Result { get; set; } = new ClusterViewModel();

        public int ClusterId { get; set; }
    }
}
