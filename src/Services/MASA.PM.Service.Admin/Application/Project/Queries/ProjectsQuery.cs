namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectsQuery(int? EnvironmentClusterId, Guid? TeamId) : Query<List<ProjectDto>>
    {
        public override List<ProjectDto> Result { get; set; } = new List<ProjectDto>();
    }
}
