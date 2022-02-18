namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record UpdateAppCommand(UpdateAppModel UpdateAppModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
