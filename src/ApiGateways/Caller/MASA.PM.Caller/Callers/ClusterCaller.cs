using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Caller.Callers
{
    public class ClusterCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/cluster";

        public ClusterCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(ClusterCaller);
        }

        protected override string BaseAddress { get; set; } = AppSettings.Get("ServiceBaseUrl");

        public async Task<List<ClusterDto>> GetListByEnvIdAsync(int envId)
        {
            var result = await CallerProvider.GetAsync<List<ClusterDto>>($"/api/v1/{envId}/cluster");

            return result ?? new();
        }

        public async Task<List<ClusterDto>> GetListAsync()
        {
            var result = await CallerProvider.GetAsync<List<ClusterDto>>($"/api/v1/cluster");

            return result ?? new();
        }

        public async Task<ClusterDetailDto> GetAsync(int Id)
        {
            var result = await CallerProvider.GetAsync<ClusterDetailDto>($"{_prefix}/{Id}");

            return result ?? new();
        }

        public async Task<List<EnvironmentClusterDto>> GetEnvironmentClusters()
        {
            var result = await CallerProvider.GetAsync<List<EnvironmentClusterDto>>($"/api/v1/envClusters");

            return result ?? new();
        }

        public async Task<ClusterDto> AddAsync(AddClusterWhitEnvironmentsDto model)
        {
            return await CallerProvider.PostAsync<AddClusterWhitEnvironmentsDto, ClusterDto>(_prefix, model) ?? new();
        }

        public async Task UpdateAsync(UpdateClusterDto model)
        {
            await CallerProvider.PutAsync(_prefix, model);
        }

        public async Task DeleteAsync(int envClusterId)
        {
            await CallerProvider.DeleteAsync(_prefix, envClusterId);
        }
    }
}
