namespace MASA.PM.Service.Admin.Application.Environment.Commands
{
    public record AddEnvironmentCommand(AddEnvironmentWhitClustersModel EnvironmentWhitClusterModel) : Command, ITransaction
    {
        public IUnitOfWork? UnitOfWork { get; set; }
    }
}
