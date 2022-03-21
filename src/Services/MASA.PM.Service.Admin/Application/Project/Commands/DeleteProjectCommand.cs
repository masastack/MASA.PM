namespace MASA.PM.Service.Admin.Application.Project.Commands
{
    public record DeleteProjectCommand(int ProjectId) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
