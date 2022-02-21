namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record InitEnvironmentClusterCommand(InitModel InitModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
