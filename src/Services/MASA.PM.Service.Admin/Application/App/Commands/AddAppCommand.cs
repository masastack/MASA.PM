namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record AddAppCommand(AddAppModel AppModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
