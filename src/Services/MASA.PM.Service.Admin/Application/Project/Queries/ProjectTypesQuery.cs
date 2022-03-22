namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectTypesQuery : Query<List<ProjectTypesDto>>
    {
        public override List<ProjectTypesDto> Result { get; set; } = new();
    }
}
