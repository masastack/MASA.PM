namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record InitEnvironmentClusterCommand(InitDto InitModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
