using MASA.PM.Contracts.Admin.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Caller.Callers
{
    public class AppCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/app";

        public AppCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(AppCaller);
        }

        protected override string BaseAddress { get; set; } = "http://localhost:6030";

        public async Task<List<AppDto>> GetListAsync()
        {
            var result = await CallerProvider.GetAsync<List<AppDto>>("/api/v1/app");

            return result;
        }

        public async Task<List<AppDto>> GetListByProjectIdAsync(List<int> projectIds)
        {
            var result = await CallerProvider.PostAsync<List<int>, List<AppDto>>($"/api/v1/projects/app", projectIds);

            return result;
        }

        public async Task AddAsync(AddAppDto model)
        {
            await CallerProvider.PostAsync($"{_prefix}", model);
        }

        public async Task AddRelationAppAsync(AddRelationAppDto model)
        {
            await CallerProvider.PostAsync($"{_prefix}/envClusterprojectApp", model);
        }

        public async Task UpdateAsync(UpdateAppDto model)
        {
            await CallerProvider.PutAsync($"{_prefix}", model);
        }

        public async Task DeleteAsync(RemoveAppDto model)
        {
            await CallerProvider.DeleteAsync(_prefix, model);
        }
    }
}
