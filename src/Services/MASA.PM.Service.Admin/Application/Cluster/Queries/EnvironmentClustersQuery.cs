namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record EnvironmentClustersQuery(int? EnvId) : Query<List<ClusterDto>>
    {
        public override List<ClusterDto> Result { get; set; } = new();

        public List<EnvironmentClusterDto> EnvironmentClusters { get; set; } = new();
    }
}
