namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record AddRelationAppCommand(AddRelationAppModel RelationAppModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
