namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record UpdateClusterCommand(UpdateClusterDto UpdateClusterModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
