namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record EnvironmentClustersQuery(int? EnvId) : Query<List<ClustersViewModel>>
    {
        public override List<ClustersViewModel> Result { get; set; } = new();

        public List<EnvironmentClusterViewModel> EnvironmentClusters { get; set; } = new();
    }
}
