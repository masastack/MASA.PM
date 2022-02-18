namespace MASA.PM.Service.Admin.Application.Cluster.Queries
{
    public record ClustersQuery : Query<List<ClustersViewModel>>
    {
        public override List<ClustersViewModel> Result { get; set; } = new();
    }
}
