namespace MASA.PM.Service.Admin.Application.Environment.Queries
{
    public record EnvsQuery(string EnvName): Query<List<ProjectModel>>
    {
        public override List<ProjectModel> Result { get; set; } = new();
    }
}
