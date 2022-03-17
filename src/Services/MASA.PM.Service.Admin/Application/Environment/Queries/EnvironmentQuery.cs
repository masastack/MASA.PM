namespace MASA.PM.Service.Admin.Application.Environment.Queries
{
    public record EnvironmentQuery : Query<EnvironmentDetailDto>
    {
        public int EnvironmentId { get; set; }

        public override EnvironmentDetailDto Result { get; set; } = default!;
    }
}
