namespace MASA.PM.Service.Admin.Application.Project.Commands
{
    public record UpdateProjectCommand(UpdateProjectModel ProjectModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
