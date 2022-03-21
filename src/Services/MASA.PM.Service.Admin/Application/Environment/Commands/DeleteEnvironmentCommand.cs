namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record DeleteEnvironmentCommand(int EnvironmentId) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
