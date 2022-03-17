namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record UpdateAppCommand(UpdateAppDto UpdateAppModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
