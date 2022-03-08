using MASA.PM.Contracts.Base.Model;
using MASA.PM.Contracts.Base.ViewModel;
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

        public async Task<List<AppViewModel>> GetListByProjectIdAsync(List<int> projectIds)
        {
            var result = await CallerProvider.PostAsync<List<int>, List<AppViewModel>>($"/api/v1/projects/app", projectIds);

            return result;
        }

        public async Task AddAsync(AddAppModel model)
        {
            await CallerProvider.PostAsync($"{_prefix}", model);
        }

        public async Task UpdateAsync(UpdateAppModel model)
        {
            await CallerProvider.PutAsync($"{_prefix}", model);
        }

        public async Task DeleteAsync(int Id)
        {
            await CallerProvider.DeleteAsync(_prefix, Id);
        }
    }
}
