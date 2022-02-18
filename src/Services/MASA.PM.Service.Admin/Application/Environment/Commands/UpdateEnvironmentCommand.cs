namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record UpdateEnvironmentCommand(UpdateEnvironmentModel EnvironmentModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
