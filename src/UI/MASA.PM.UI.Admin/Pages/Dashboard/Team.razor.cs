using MASA.PM.Caller.Callers;
using MASA.PM.Contracts.Base.Model;
using MASA.PM.Contracts.Base.ViewModel;

namespace MASA.PM.UI.Admin.Pages.Dashboard
{
    public partial class Team : ProCompontentBase
    {
        public List<ProjectsViewModel> _projects = new();
        public List<AppViewModel> _apps = new();
        private string _projectName = "";
        private ProjectViewModel _projectDetail = new();
        private DataModal<UpdateProjectModel> _projectFormModel = new();
        private List<EnvironmentClusterViewModel> allEnvClusters = new();
        private int _selectProjectId;

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
                await InitDataAsync();
                StateHasChanged();
            }
        }

        private async Task InitDataAsync()
        {
            _projects = await ProjectCaller.GetListAsync();
            var projectIds = _projects.Select(project => project.Id).ToList();
            _apps = await AppCaller.GetListByProjectIdAsync(projectIds);
        }

        private async Task SearchProject(KeyboardEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(_projectName))
            {
                _projects = _projects.Where(project => project.Name.ToLower().Contains(_projectName.ToLower())).ToList();
            }
            else
            {
                await InitDataAsync();
            }
        }

        private async Task<ProjectViewModel> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);

            return _projectDetail;
        }

        private async Task EditProjectAsync(int projectId)
        {
            _selectProjectId = projectId;
            var project = await GetProjectAsync(projectId);
            await ShowProjectModalAsync(new UpdateProjectModel
            {
                ProjectId = project.Id,
                Name = project.Name,
                Description = project.Description,
                EnvironmentClusterIds = project.EnvironmentClusterIds
            });
        }

        private async Task ShowProjectModalAsync(UpdateProjectModel? model = null)
        {
            allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            if (model == null)
            {
                _projectFormModel.Show();
            }
            else
            {
                _projectFormModel.Show(model);
            }

            //TODO: get team by auth sdk;
            await Task.Delay(0);
        }

        private async Task SubmitProject()
        {
            if (!_projectFormModel.HasValue)
            {
                await ProjectCaller.AddAsync(_projectFormModel.Data);
            }
            else
            {
                await ProjectCaller.UpdateAsync(_projectFormModel.Data);
            }

            _projects = await ProjectCaller.GetListAsync();
            _projectFormModel.Hide();
        }

        private async Task DeleteProject()
        {
            var deleteProject = _projects.First(project => project.Id == _selectProjectId);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteProject.Name}]项目吗？", async (c) =>
            {
                await ProjectCaller.DeleteAsync(_selectProjectId);

                _projects.Remove(deleteProject);

                _projectFormModel.Hide();
            });
        }
    }
}
