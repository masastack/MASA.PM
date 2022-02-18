namespace MASA.PM.Service.Admin.Application.App.Queries
{
    public record AppQuery(bool IsHaveEnvironmentClusterInfo, int AppId) : Query<AppViewModel>
    {
        public override AppViewModel Result { get; set; } = new AppViewModel();
    }
}
