using MASA.PM.Service.Admin.Application.Cluster.Commands;
using MASA.PM.Service.Admin.Application.Cluster.Queries;
using MASA.PM.Service.Admin.Application.Project.Commands;
using MASA.PM.Service.Admin.Application.Project.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MASA.PM.Service.Admin.Services
{
    public class ProjectService : ServiceBase
    {
        public ProjectService(IServiceCollection services) : base(services)
        {
            App.MapPost("/api/v1/project", AddAsync);
            App.MapGet("/api/v1/project/teamProjects/{teamId}", GetListByTeamId);
            App.MapGet("/api/v1/{environmentClusterId}/project", GetListByEnvironmentClusterId);
            App.MapGet("/api/v1/project/{Id}", GetAsync);
            App.MapPut("/api/v1/project", UpdateAsync);
            App.MapDelete("/api/v1/project", DeleteAsync);
        }

        public async Task AddAsync(IEventBus eventBus, AddProjectModel model)
        {
            var command = new AddProjectCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task<List<ProjectsViewModel>> GetListByEnvironmentClusterId(IEventBus eventBus, int environmentClusterId)
        {
            var query = new ProjectsQuery(environmentClusterId, null);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<ProjectsViewModel>> GetListByTeamId(IEventBus eventBus, Guid teamId)
        {
            var query = new ProjectsQuery(null, teamId);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<ProjectViewModel> GetAsync(IEventBus eventBus, int Id)
        {
            var query = new ProjectQuery
            {
                ProjectId = Id
            };
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task UpdateAsync(IEventBus eventBus, UpdateProjectModel model)
        {
            var command = new UpdateProjectCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task DeleteAsync(IEventBus eventBus, [FromBody] int Id)
        {
            var command = new DeleteProjectCommand(Id);
            await eventBus.PublishAsync(command);
        }
    }
}
