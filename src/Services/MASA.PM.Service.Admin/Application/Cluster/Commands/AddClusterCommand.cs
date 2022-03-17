namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record AddClusterCommand(AddClusterWhitEnvironmentsDto ClustersWhitEnvironmentModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }

        public ClusterDto Result { get; set; } = default!;
    }
}
