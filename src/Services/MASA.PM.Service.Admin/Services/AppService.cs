using MASA.PM.Service.Admin.Application.App.Commands;
using MASA.PM.Service.Admin.Application.App.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MASA.PM.Service.Admin.Services
{
    public class AppService : ServiceBase
    {
        public AppService(IServiceCollection services) : base(services)
        {
            App.MapPost("/api/v1/app", AddAsync);
            App.MapPost("/api/v1/app/envClusterprojectApp", AddEnvClusterProjectApp);
            App.MapGet("/api/v1/app", GetListAsync);
            App.MapPost("/api/v1/projects/app", GetListByProjectIdsAsync);
            App.MapGet("/api/v1/app/{Id}", GetAsync);
            App.MapGet("/api/v1/appWhitEnvCluster/{Id}", GetWithEnvironmentClusterAsync);
            App.MapPut("/api/v1/app", UpdateAsync);
            App.MapDelete("/api/v1/app", DeleteAsync);
        }

        public async Task AddAsync(IEventBus eventBus, AddAppModel model)
        {
            var command = new AddAppCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task AddEnvClusterProjectApp(IEventBus eventBus, AddRelationAppModel model)
        {
            var command = new AddRelationAppCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task<List<AppViewModel>> GetListAsync(IEventBus eventBus)
        {
            var query = new AppsQuery(new List<int>());
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<AppViewModel>> GetListByProjectIdsAsync(IEventBus eventBus, [FromBody] List<int> projectIds)
        {
            var query = new AppsQuery(projectIds);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<AppViewModel> GetWithEnvironmentClusterAsync(IEventBus eventBus, int Id)
        {
            var query = new AppQuery(true, Id);

            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<AppViewModel> GetAsync(IEventBus eventBus, int Id)
        {
            var query = new AppQuery(false, Id);

            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task UpdateAsync(IEventBus eventBus, UpdateAppModel model)
        {
            var command = new UpdateAppCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task DeleteAsync(IEventBus eventBus, [FromBody] RemoveAppModel model)
        {
            var command = new DeleteAppCommand(model.AppId, model.ProjectId);
            await eventBus.PublishAsync(command);
        }
    }
}
