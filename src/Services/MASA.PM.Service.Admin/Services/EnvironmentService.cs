using MASA.PM.Service.Admin.Application.Environment.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MASA.PM.Service.Admin.Services
{
    public class EnvironmentService : ServiceBase
    {
        public EnvironmentService(IServiceCollection services) : base(services)
        {
            App.MapPost("/api/v1/env", AddAsync);
            App.MapGet("api/v1/env", GetList);
            App.MapGet("api/v1/env/{Id}", GetAsync);
            App.MapPut("/api/v1/env", UpdateAsync);
            App.MapDelete("/api/v1/env/{Id}", DeleteAsync);
        }

        public async Task AddAsync(IEventBus eventBus, AddEnvironmentWhitClustersModel model)
        {
            var command = new AddEnvironmentCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task<List<EnvironmentsViewModel>> GetList(IEventBus eventBus)
        {
            var query = new EnvironmentsQuery();
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<EnvironmentViewModel> GetAsync(IEventBus eventBus, int Id)
        {
            var query = new EnvironmentQuery
            {
                EnvironmentId = Id
            };
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task UpdateAsync(IEventBus eventBus, UpdateEnvironmentModel model)
        {
            var command = new UpdateEnvironmentCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task DeleteAsync(IEventBus eventBus,int Id)
        {
            var command = new DeleteEnvironmentCommand(Id);
            await eventBus.PublishAsync(command);
        }
    }
}
