namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectsQuery(int EnvironmentClusterId) : Query<List<ProjectViewModel>>
    {
        public override List<ProjectViewModel> Result { get; set; } = new List<ProjectViewModel>();
    }
}
