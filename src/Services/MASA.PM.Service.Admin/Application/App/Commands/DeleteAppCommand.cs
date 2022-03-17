namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record DeleteAppCommand(int AppId,int ProjectId) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
