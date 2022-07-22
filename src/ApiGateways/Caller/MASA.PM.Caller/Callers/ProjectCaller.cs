// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class ProjectCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/project";

        public ProjectCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(ProjectCaller);
        }

        protected override string BaseAddress { get; set; } = AppSettings.Get("ServiceBaseUrl");

        public async Task<List<ProjectDto>> GetListByTeamIdsAsync(IEnumerable<Guid> teamIds)
        {
            var result = await CallerProvider.PostAsync<List<ProjectDto>>($"{_prefix}/teamProjects", teamIds);

            return result ?? new();
        }

        public async Task<List<ProjectDto>> GetListByEnvClusterIdAsync(int environmentClusterId)
        {
            var result = await CallerProvider.GetAsync<List<ProjectDto>>($"/api/v1/{environmentClusterId}/project");

            return result ?? new();
        }

        public async Task<ProjectDetailDto> GetAsync(int Id)
        {
            var result = await CallerProvider.GetAsync<ProjectDetailDto>($"{_prefix}/{Id}");

            return result ?? new();
        }

        public async Task<List<ProjectTypesDto>> GetProjectTypesAsync()
        {
            var result = await CallerProvider.GetAsync<List<ProjectTypesDto>>($"{_prefix}/projectType");

            return result ?? new();
        }

        public async Task AddAsync(AddProjectDto model)
        {
            await CallerProvider.PostAsync(_prefix, model);
        }

        public async Task UpdateAsync(UpdateProjectDto model)
        {
            await CallerProvider.PutAsync(_prefix, model);
        }

        public async Task DeleteAsync(int Id)
        {
            await CallerProvider.DeleteAsync(_prefix, Id);
        }
    }
}
