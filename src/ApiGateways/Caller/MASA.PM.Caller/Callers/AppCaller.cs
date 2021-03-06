// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class AppCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/app";

        public AppCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(AppCaller);
        }

        protected override string BaseAddress { get; set; } = AppSettings.Get("ServiceBaseUrl");

        public async Task<List<AppDto>> GetListAsync()
        {
            var result = await CallerProvider.GetAsync<List<AppDto>>("/api/v1/app");

            return result ?? new();
        }

        public async Task<List<AppDto>> GetListByProjectIdAsync(List<int> projectIds)
        {
            var result = await CallerProvider.PostAsync<List<int>, List<AppDto>>($"/api/v1/projects/app", projectIds);

            return result ?? new();
        }

        public async Task AddAsync(AddAppDto model)
        {
            await CallerProvider.PostAsync(_prefix, model);
        }

        public async Task AddRelationAppAsync(AddRelationAppDto model)
        {
            await CallerProvider.PostAsync($"{_prefix}/envClusterprojectApp", model);
        }

        public async Task UpdateAsync(UpdateAppDto model)
        {
            await CallerProvider.PutAsync(_prefix, model);
        }

        public async Task RemoveAsync(int id)
        {
            await CallerProvider.DeleteAsync($"{_prefix}/{id}", null);
        }
    }
}
