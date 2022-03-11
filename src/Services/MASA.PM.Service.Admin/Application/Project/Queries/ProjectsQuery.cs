namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectsQuery(int? EnvironmentClusterId) : Query<List<ProjectsViewModel>>
    {
        public override List<ProjectsViewModel> Result { get; set; } = new List<ProjectsViewModel>();
    }
}
