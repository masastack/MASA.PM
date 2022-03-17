namespace MASA.PM.Service.Admin.Application.Environment.Queries
{
    public record EnvironmentsQuery : Query<List<EnvironmentDto>>
    {
        public override List<EnvironmentDto> Result { get; set; } = new();
    }
}
