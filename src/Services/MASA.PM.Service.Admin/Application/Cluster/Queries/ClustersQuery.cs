namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record ClustersQuery : Query<List<ClusterDto>>
    {
        public override List<ClusterDto> Result { get; set; } = new();
    }
}
