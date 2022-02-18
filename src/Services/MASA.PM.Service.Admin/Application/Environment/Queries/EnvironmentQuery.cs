namespace MASA.PM.Service.Admin.Application.Environment.Queries
{
    public record EnvironmentQuery : Query<EnvironmentViewModel>
    {
        public int EnvironmentId { get; set; }

        public override EnvironmentViewModel Result { get; set; } = default!;
    }
}
