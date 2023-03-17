// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class ClusterCaller : PmHttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/cluster";

        public ClusterCaller(PMApiGatewayOptions options) : base(options)
        {
        }

        public async Task<List<ClusterDto>> GetListByEnvIdAsync(int envId)
        {
            var result = await Caller.GetAsync<List<ClusterDto>>($"/api/v1/{envId}/cluster");

            return result ?? new();
        }

        public async Task<List<ClusterDto>> GetListAsync()
        {
            var result = await Caller.GetAsync<List<ClusterDto>>($"/api/v1/cluster");

            return result ?? new();
        }

        public async Task<ClusterDetailDto> GetAsync(int Id)
        {
            var result = await Caller.GetAsync<ClusterDetailDto>($"{_prefix}/{Id}");

            return result ?? new();
        }

        public async Task<List<EnvironmentClusterDto>> GetEnvironmentClusters()
        {
            var result = await Caller.GetAsync<List<EnvironmentClusterDto>>($"/api/v1/envClusters");

            return result ?? new();
        }

        public async Task<ClusterDto> AddAsync(AddClusterWhitEnvironmentsDto model)
        {
            return await Caller.PostAsync<AddClusterWhitEnvironmentsDto, ClusterDto>(_prefix, model) ?? new();
        }

        public async Task UpdateAsync(UpdateClusterDto model)
        {
            await Caller.PutAsync(_prefix, model);
        }

        public async Task RemoveAsync(int id)
        {
            await Caller.DeleteAsync($"{_prefix}/{id}", null);
        }
    }
}
