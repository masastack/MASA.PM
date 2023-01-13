// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class ProjectCaller : PmHttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/project";

        public ProjectCaller(
            IServiceProvider serviceProvider,
            PMApiGatewayOptions options) : base(serviceProvider, options)
        {
            Name = nameof(ProjectCaller);
        }

        public async Task<List<ProjectDto>> GetListByTeamIdsAsync(IEnumerable<Guid> teamIds)
        {
            var result = await Caller.PostAsync<List<ProjectDto>>($"{_prefix}/teamProjects", teamIds);

            return result ?? new();
        }

        public async Task<List<ProjectDto>> GetListByEnvClusterIdAsync(int environmentClusterId)
        {
            var result = await Caller.GetAsync<List<ProjectDto>>($"/api/v1/{environmentClusterId}/project");

            return result ?? new();
        }

        public async Task<ProjectDetailDto> GetAsync(int Id)
        {
            var result = await Caller.GetAsync<ProjectDetailDto>($"{_prefix}/{Id}");

            return result ?? new();
        }

        public async Task<List<ProjectTypesDto>> GetProjectTypesAsync()
        {
            var result = await Caller.GetAsync<List<ProjectTypesDto>>($"{_prefix}/projectType");

            return result ?? new();
        }

        public async Task<bool> IsExistProjectInCluster(int clusterId)
        {
            var result = await Caller.GetAsync<bool>($"{_prefix}/isExistProjectInCluster/{clusterId}");

            return result;
        }

        public async Task AddAsync(AddProjectDto model)
        {
            await Caller.PostAsync(_prefix, model);
        }

        public async Task UpdateAsync(UpdateProjectDto model)
        {
            await Caller.PutAsync(_prefix, model);
        }

        public async Task DeleteAsync(int Id)
        {
            await Caller.DeleteAsync(_prefix, Id);
        }
    }
}
