using MASA.PM.Contracts.Base.Model;
using MASA.PM.Contracts.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Caller.Environment
{
    public class EnvironmentCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/env";

        public EnvironmentCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(EnvironmentCaller);
        }

        protected override string BaseAddress { get; set; } = "http://localhost:6030";

        //protected override string AppId { get; set; } = "masa-pm-service-admin";

        public async Task<string> GetAsync(int Id)
        {
            var data = await CallerProvider.GetAsync<string>($"{_prefix}{Id}");

            return data;
        }

        public async Task InitAsync(InitModel model)
        {
            await CallerProvider.PostAsync($"{_prefix}/init", model);
        }

        public async Task<List<EnvironmentsViewModel>> GetListAsync()
        {
            var result = await CallerProvider.GetAsync<List<EnvironmentsViewModel>>($"{_prefix}");

            return result;
        }
    }
}
