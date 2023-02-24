// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class EnvironmentCaller : PmHttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/env";

        public EnvironmentCaller(
            IServiceProvider serviceProvider,
            PMApiGatewayOptions options) : base(serviceProvider, options)
        {
        }

        public async Task<EnvironmentDetailDto> GetAsync(int Id)
        {
            var result = await Caller.GetAsync<EnvironmentDetailDto>($"{_prefix}/{Id}");

            return result ?? new();
        }

        public async Task InitAsync(InitDto model)
        {
            await Caller.PostAsync($"{_prefix}/init", model);
        }

        public async Task<List<EnvironmentDto>> GetListAsync()
        {
            var result = await Caller.GetAsync<List<EnvironmentDto>>($"{_prefix}");

            return result ?? new();
        }

        public async Task<EnvironmentDto> AddAsync(AddEnvironmentWhitClustersDto model)
        {
            return await Caller.PostAsync<AddEnvironmentWhitClustersDto, EnvironmentDto>(_prefix, model) ?? new();
        }

        public async Task UpdateAsync(UpdateEnvironmentDto model)
        {
            await Caller.PutAsync(_prefix, model);
        }

        public async Task RemoveAsync(int Id)
        {
            await Caller.DeleteAsync(_prefix, Id);
        }
    }
}
