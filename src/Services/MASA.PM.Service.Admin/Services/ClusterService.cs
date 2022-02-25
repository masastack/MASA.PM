using MASA.PM.Service.Admin.Application.Cluster.Commands;
using MASA.PM.Service.Admin.Application.Cluster.Queries;

namespace MASA.PM.Service.Admin.Services
{
    public class ClusterService : ServiceBase
    {
        public ClusterService(IServiceCollection services) : base(services)
        {
            App.MapPost("/api/v1/cluster", AddAsync);
            App.MapGet("/api/v1/cluster", GetList);
            App.MapGet("/api/v1/{envId}/cluster", GetListByEnvId);
            App.MapGet("/api/v1/cluster/{Id}", GetAsync);
            App.MapPut("/api/v1/cluster", UpdateAsync);
            App.MapDelete("/api/v1/cluster/{Id}", DeleteAsync);
        }

        public async Task AddAsync(IEventBus eventBus, AddClusterWhitEnvironmentsModel model)
        {
            var command = new AddClusterCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task<List<ClustersViewModel>> GetList(IEventBus eventBus)
        {
            var query = new ClustersQuery();
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<ClustersViewModel>> GetListByEnvId(IEventBus eventBus, int envId)
        {
            var query = new EnvironmentClustersQuery(envId);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<ClusterViewModel> GetAsync(IEventBus eventBus, int Id)
        {
            var query = new ClusterQuery
            {
                ClusterId = Id
            };
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task UpdateAsync(IEventBus eventBus, UpdateClusterModel model)
        {
            var command = new UpdateClusterCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task DeleteAsync(IEventBus eventBus, int Id)
        {
            var command = new DeleteClusterCommand(Id);
            await eventBus.PublishAsync(command);
        }
    }
}
