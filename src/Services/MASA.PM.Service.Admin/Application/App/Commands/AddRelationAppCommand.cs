namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record AddRelationAppCommand(AddRelationAppDto RelationAppModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
