namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectAppsQuery(string EnvName) : Query<List<ProjectModel>>
    {
        public override List<ProjectModel> Result { get; set; } = new();
    }
}
