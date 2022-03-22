namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record AddEnvironmentCommand(AddEnvironmentWhitClustersDto EnvironmentWhitClusterModel) : Command
    {
        public EnvironmentDto Result { get; set; } = default!;
    }
}
