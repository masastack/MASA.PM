namespace MASA.PM.Service.Admin.Application.Environment.Queries
{
    public record EnvironmentsQuery : Query<List<EnvironmentsViewModel>>
    {
        public override List<EnvironmentsViewModel> Result { get; set; } = new();
    }
}
