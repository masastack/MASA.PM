namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record UpdateClusterCommand(UpdateClusterModel UpdateClusterModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
