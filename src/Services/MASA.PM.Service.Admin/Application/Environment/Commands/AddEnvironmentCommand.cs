namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record AddEnvironmentCommand(AddEnvironmentWhitClustersDto EnvironmentWhitClusterModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }

        public EnvironmentDto Result { get; set; } = default!;
    }
}
