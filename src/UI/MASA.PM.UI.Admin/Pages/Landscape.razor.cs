using MASA.Blazor.Experimental.Components;
using MASA.PM.Caller.Callers;
using MASA.PM.Contracts.Base.Model;
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
        private DataModal<UpdateEnvironmentModel> _envFormModel = new();
        private List<ClustersViewModel> _allClusters = new();

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
            _clusters = await ClusterCaller.GetListByEnvIdAsync(envId);
            if (_clusters.Any())
            {
                _selectEnvClusterId = _clusters[0].EnvironmentClusterId;
                _projects = await GetProjectByEnvClusterIdAsync(_clusters[0].EnvironmentClusterId);
            }

            return _clusters;
        }

        private async Task<List<ProjectViewModel>> GetProjectByEnvClusterIdAsync(int envClusterId)
        {
            _selectEnvClusterId = envClusterId;
            _projects = await ProjectCaller.GetListByEnvIdAsync(envClusterId);
            if (_projects.Any())
            {
                _apps = await GetAppByProjectIdAsync(_projects.Select(project => project.Id));
                _projectApps = _apps.Where(app => _projects.Select(p => p.Id).Contains(app.ProjectId)).ToList();
            }

            return _projects;
        }

        private async Task<List<AppViewModel>> GetAppByProjectIdAsync(IEnumerable<int> projectIds)
        {
            var result = await AppCaller.GetListByProjectIdAsync(projectIds.ToList());

            return result;
        }

        private async Task EditEnvAsync(int envId)
        {
            var env = await GetEnvAsync(envId);
            _envFormModel.Show(new UpdateEnvironmentModel { });
        }

        private async Task<EnvironmentViewModel> GetEnvAsync(int envId)
        {
            var env = await EnvironmentCaller.GetAsync(envId);
            return env;
        }

        private async Task ShowEnvModal()
        {
            _envFormModel.Show();
            _allClusters = await ClusterCaller.GetList();
        }

        private async Task SubmitEnv()
        {
            if (!_envFormModel.HasValue)
            {
                var newEnv = await EnvironmentCaller.AddAsync(_envFormModel.Data);
                _environments.Add(newEnv);
            }
            else
            {

            }

            _envFormModel.Hide();
        }
    }
}
