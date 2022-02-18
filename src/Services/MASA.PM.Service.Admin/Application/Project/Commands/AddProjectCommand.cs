namespace MASA.PM.Service.Admin.Application.Project.Commands
{
    public record AddProjectCommand(AddProjectModel ProjectModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
