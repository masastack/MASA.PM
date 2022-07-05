// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

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
            App.MapGet("/api/v1/projects", GetListAsync);
            App.MapGet("/api/v1/project/teamProjects/{teamId}", GetListByTeamId);
            App.MapGet("/api/v1/{environmentClusterId}/project", GetListByEnvironmentClusterId);
            App.MapGet("/api/v1/project/{Id}", GetAsync);
            App.MapGet("/api/v1/project/projectType", GetProjectTypes);
            App.MapGet("/api/v1/projectwithapps/{envName}", GetListByEnvName);
            App.MapPut("/api/v1/project", UpdateAsync);
            App.MapDelete("/api/v1/project", RemoveAsync);
        }

        public async Task AddAsync(IEventBus eventBus, AddProjectDto model)
        {
            var command = new AddProjectCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task<List<ProjectDto>> GetListByEnvironmentClusterId(IEventBus eventBus, int environmentClusterId)
        {
            var query = new ProjectsQuery(environmentClusterId, null);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<ProjectDto>> GetListByTeamId(IEventBus eventBus, Guid teamId)
        {
            var query = new ProjectsQuery(null, teamId);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<ProjectDto>> GetListAsync(IEventBus eventBus)
        {
            var query = new ProjectListQuery();
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<ProjectModel>> GetListByEnvName(IEventBus eventBus, string envName)
        {
            var query = new ProjectAppsQuery(envName);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<ProjectDetailDto> GetAsync(IEventBus eventBus, int Id)
        {
            var query = new ProjectQuery
            {
                ProjectId = Id
            };
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<List<ProjectTypesDto>> GetProjectTypes(IEventBus eventBus)
        {
            var query = new ProjectTypesQuery();

            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task UpdateAsync(IEventBus eventBus, UpdateProjectDto model)
        {
            var command = new UpdateProjectCommand(model);
            await eventBus.PublishAsync(command);
        }

        public async Task RemoveAsync(IEventBus eventBus, [FromBody] int Id)
        {
            var command = new DeleteProjectCommand(Id);
            await eventBus.PublishAsync(command);
        }
    }
}
