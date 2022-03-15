using MASA.PM.Contracts.Base.Model;
using MASA.PM.Contracts.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Caller.Callers
{
    public class ProjectCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/project";

        public ProjectCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(ProjectCaller);
        }

        protected override string BaseAddress { get; set; } = "http://localhost:6030";

        public async Task<List<ProjectsViewModel>> GetListByTeamIdAsync(Guid teamId)
        {
            var result = await CallerProvider.GetAsync<List<ProjectsViewModel>>($"{_prefix}/teamProjects/{teamId}");

            return result;
        }

        public async Task<List<ProjectsViewModel>> GetListByEnvIdAsync(int envClusterId)
        {
            var result = await CallerProvider.GetAsync<List<ProjectsViewModel>>($"/api/v1/{envClusterId}/project");

            return result;
        }

        public async Task<ProjectViewModel> GetAsync(int Id)
        {
            var result = await CallerProvider.GetAsync<ProjectViewModel>($"{_prefix}/{Id}");

            return result;
        }

        public async Task AddAsync(AddProjectModel model)
        {
            await CallerProvider.PostAsync($"{_prefix}", model);
        }

        public async Task UpdateAsync(UpdateProjectModel model)
        {
            await CallerProvider.PutAsync($"{_prefix}", model);
        }

        public async Task DeleteAsync(int Id)
        {
            await CallerProvider.DeleteAsync(_prefix, Id);
        }
    }
}
