using MASA.Blazor.Experimental.Components;
using MASA.PM.Caller.Callers;
using MASA.PM.Contracts.Base.ViewModel;

namespace MASA.PM.UI.Admin.Pages
{
    public partial class Landscape
    {
        private StringNumber _selectedEnvId = 0;
        private StringNumber _selectEnvClusterId = 0;
        private List<EnvironmentsViewModel> _environments = new();
        private List<ClustersViewModel> _clusters = new();
        private List<ProjectViewModel> _projects = new();
        private List<AppViewModel> _apps = new();
        private List<AppViewModel> _projectApps = new();

        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        [Inject]
        public ClusterCaller ClusterCaller { get; set; } = default!;

        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public AppCaller AppCaller { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _environments = await EnvironmentCaller.GetListAsync();
                if (_environments.Any())
                {
                    _selectedEnvId = _environments[0].Id;
                    _clusters = await GetClustersByEnvIdAsync(_environments[0].Id);
                }

                StateHasChanged();
            }
        }

        private async Task<List<ClustersViewModel>> GetClustersByEnvIdAsync(int envId)
        {
            _selectedEnvId = envId;
            var clusters = await ClusterCaller.GetListByEnvIdAsync(envId);
            if (clusters.Any())
            {
                _selectEnvClusterId = clusters[0].EnvironmentClusterId;

                _selectEnvClusterId = clusters[0].EnvironmentClusterId;
                _projects = await GetProjectByEnvClusterIdAsync(clusters[0].EnvironmentClusterId);
            }

            return clusters;
        }

        private async Task<List<ProjectViewModel>> GetProjectByEnvClusterIdAsync(int envClusterId)
        {
            _selectEnvClusterId = envClusterId;
            var projects = await ProjectCaller.GetListByEnvIdAsync(envClusterId);
            if (projects.Any())
            {
                _apps = await GetAppByProjectIdAsync(projects.Select(project => project.Id));
                _projectApps = _apps.Where(app =>projects.Select(p=>p.Id).Contains(app.ProjectId)).ToList();
            }

            return projects;
        }

        private async Task<List<AppViewModel>> GetAppByProjectIdAsync(IEnumerable<int> projectIds)
        {
            var result = await AppCaller.GetListByProjectIdAsync(projectIds.ToList());

            return result;
        }

        private async Task ChangeEnv()
        {

        }
    }
}
