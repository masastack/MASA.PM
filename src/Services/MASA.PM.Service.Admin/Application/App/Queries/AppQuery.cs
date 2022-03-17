namespace MASA.PM.Service.Admin.Application.App.Queries
{
    public record AppQuery(bool IsHaveEnvironmentClusterInfo, int AppId) : Query<AppDto>
    {
        public override AppDto Result { get; set; } = new AppDto();
    }
}
