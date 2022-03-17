namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record AddAppCommand(AddAppDto AppModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
