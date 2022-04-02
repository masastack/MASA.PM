namespace MASA.PM.Service.Admin.Services.ThirdPartyInvocation
{
    public class EnvironmentService : ServiceBase
    {
        public EnvironmentService(IServiceCollection services) : base(services)
        {
            App.MapGet("third-party/api/v1/env/{envName}", GetEnvByName);
        }

        public async Task<List<ProjectModel>> GetEnvByName(IEventBus eventBus, string envName)
        {
            var query = new EnvsQuery(envName);
            await eventBus.PublishAsync(query);

            return query.Result;
        }
    }
}
