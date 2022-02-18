namespace MASA.PM.Service.Admin.Application.Cluster.Commands
{
    public record DeleteClusterCommand(int ClusterId) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
