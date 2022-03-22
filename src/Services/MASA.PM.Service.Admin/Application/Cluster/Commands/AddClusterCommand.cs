namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record AddClusterCommand(AddClusterWhitEnvironmentsDto ClustersWhitEnvironmentModel) : Command
    {
        public ClusterDto Result { get; set; } = default!;
    }
}
