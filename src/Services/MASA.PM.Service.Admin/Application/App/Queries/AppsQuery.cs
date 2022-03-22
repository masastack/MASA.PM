namespace MASA.PM.Service.Admin.Application.App.Queries
{
    public record AppsQuery(List<int> ProjectIds) : Query<List<AppDto>>
    {
        public override List<AppDto> Result { get; set; } = new();
    }
}
