using MASA.PM.Contracts.Base.Model;
using MASA.PM.Contracts.Base.ViewModel;
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

        protected override string BaseAddress { get; set; } = "http://localhost:6030";

        public async Task<List<ClustersViewModel>> GetListByEnvIdAsync(int envId)
        {
            var result = await CallerProvider.GetAsync<List<ClustersViewModel>>($"/api/v1/{envId}/cluster");

            return result;
        }

        public async Task<List<ClustersViewModel>> GetListAsync()
        {
            var result = await CallerProvider.GetAsync<List<ClustersViewModel>>($"/api/v1/cluster");

            return result;
        }

        public async Task<ClusterViewModel> GetAsync(int Id)
        {
            var result = await CallerProvider.GetAsync<ClusterViewModel>($"{_prefix}/{Id}");

            return result;
        }

        public async Task<List<EnvironmentClusterViewModel>> GetEnvironmentClusters()
        {
            var result = await CallerProvider.GetAsync<List<EnvironmentClusterViewModel>>($"/api/v1/envClusters");

            return result;
        }

        public async Task<ClustersViewModel> AddAsync(AddClusterWhitEnvironmentsModel model)
        {
            return await CallerProvider.PostAsync<AddClusterWhitEnvironmentsModel, ClustersViewModel>(_prefix, model);
        }

        public async Task UpdateAsync(UpdateClusterModel model)
        {
            await CallerProvider.PutAsync(_prefix, model);
        }

        public async Task DeleteAsync(int envClusterId)
        {
            await CallerProvider.DeleteAsync(_prefix, envClusterId);
        }
    }
}
