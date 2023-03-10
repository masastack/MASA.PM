// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class AppCaller : PmHttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/app";

        public AppCaller(
            IServiceProvider serviceProvider,
            TokenProvider tokenProvider,
            PMApiGatewayOptions options) : base(serviceProvider, tokenProvider, options)
        {
        }

        public async Task<List<AppDto>> GetListAsync()
        {
            var result = await Caller.GetAsync<List<AppDto>>("/api/v1/app");

            return result ?? new();
        }

        public async Task<List<AppDto>> GetListByProjectIdAsync(List<int> projectIds)
        {
            var result = await Caller.PostAsync<List<int>, List<AppDto>>($"/api/v1/projects/app", projectIds);

            return result ?? new();
        }

        public async Task AddAsync(AddAppDto model)
        {
            await Caller.PostAsync(_prefix, model);
        }

        public async Task UpdateAsync(UpdateAppDto model)
        {
            await Caller.PutAsync(_prefix, model);
        }

        public async Task RemoveAsync(int id)
        {
            await Caller.DeleteAsync($"{_prefix}/{id}", null);
        }
    }
}
