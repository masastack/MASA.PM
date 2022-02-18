namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record DeleteAppCommand(int AppId) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
