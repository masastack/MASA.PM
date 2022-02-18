namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record AddClusterCommand(AddClusterWhitEnvironmentsModel ClustersWhitEnvironmentModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
