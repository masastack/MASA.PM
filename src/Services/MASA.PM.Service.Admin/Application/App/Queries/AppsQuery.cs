namespace MASA.PM.Service.Admin.Application.App.Queries
{
    public record AppsQuery(int ProjectId) : Query<List<AppViewModel>>
    {
        public override List<AppViewModel> Result { get; set; } = new();
    }
}
