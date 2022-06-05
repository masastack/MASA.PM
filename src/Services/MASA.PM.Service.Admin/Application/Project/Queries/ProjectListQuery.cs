namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectListQuery : Query<List<ProjectDto>>
    {
        public override List<ProjectDto> Result { get; set; } = new List<ProjectDto>();
    }
}
