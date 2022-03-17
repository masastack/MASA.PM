using MASA.PM.Caller.Callers;

namespace MASA.PM.UI.Admin.Pages.Home
{
    public partial class Team : ProCompontentBase
    {
        private StringNumber _curTab = 0;
        private bool _teamDetailDisabled = true;
        private List<ProjectDto> _projects = new();
        private List<AppDto> _apps = new();
        private string _projectName = "";
        private ProjectDetailDto _projectDetail = new();
        private DataModal<UpdateProjectDto> _projectFormModel = new();
        private List<EnvironmentClusterDto> allEnvClusters = new();
        private int _selectProjectId;
        private int _appCount;
        private List<AppDto> _projectApps = new();
        private string _appName = "";
        private DataModal<UpdateAppDto> _appFormModel = new();
        private List<EnvironmentClusterDto> _projectEnvClusters = new();
        private AppDto _appDetail = new();
        private int _selectAppType;
        private int _selectAppServiceType;
        private int _selectAppId;
        private int _selectProjectType;

        public Guid TeamId { get; set; } = Guid.Empty;

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
            _projects = await ProjectCaller.GetListByTeamIdAsync(TeamId);
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

        private async Task<ProjectDetailDto> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);

            return _projectDetail;
        }

        private async Task UpdateProjectAsync(int projectId)
        {
            _selectProjectId = projectId;
            var project = await GetProjectAsync(projectId);
            _selectProjectType = (int)project.Type;
            await ShowProjectModalAsync(new UpdateProjectDto
            {
                Identity = project.Identity,
                Type = project.Type,
                ProjectId = project.Id,
                Name = project.Name,
                Description = project.Description,
                EnvironmentClusterIds = project.EnvironmentClusterIds
            });
        }

        private async Task ShowProjectModalAsync(UpdateProjectDto? model = null)
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

        private async Task SubmitProjectAsync()
        {
            if (!_projectFormModel.HasValue)
            {
                _projectFormModel.Data.Type = (ProjectTypes)_selectProjectType;
                await ProjectCaller.AddAsync(_projectFormModel.Data);
            }
            else
            {
                await ProjectCaller.UpdateAsync(_projectFormModel.Data);
            }

            _projects = await ProjectCaller.GetListByTeamIdAsync(TeamId);
            _projectFormModel.Hide();
        }

        private async Task RemoveProjectAsync()
        {
            var deleteProject = _projects.First(project => project.Id == _selectProjectId);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteProject.Name}]项目吗？", async (c) =>
            {
                await ProjectCaller.DeleteAsync(_selectProjectId);

                _projects.Remove(deleteProject);

                _projectFormModel.Hide();
            });
        }

        private async Task GetProjectDetailAsync(int projectId, int appCount)
        {
            _teamDetailDisabled = false;
            _curTab = 1;

            allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            _selectProjectId = projectId;
            _appCount = appCount;
            _projectApps = _apps.Where(app => app.ProjectId == projectId).ToList();
            await GetProjectAsync(projectId);
        }

        private void ProjectHide()
        {
            _selectProjectType = 0;
            _projectFormModel.Hide();
        }

        private void SearchApp()
        {
            if (!string.IsNullOrWhiteSpace(_appName))
            {
                _projectApps = _projectApps.Where(app => app.Name.ToLower().Contains(_appName.ToLower())).ToList();
            }
            else
            {
                _projectApps = _apps.Where(app => app.ProjectId == _selectProjectId).ToList();
            }
        }

        private void UpdateApp(int appId)
        {
            _selectAppId = appId;
            _appDetail = _apps.First(app => app.Id == appId);
            _selectAppType = (int)_appDetail.Type;
            _selectAppServiceType = (int)_appDetail.ServiceType;
            ShowAppModal(new UpdateAppDto
            {
                Id = _appDetail.Id,
                Type = _appDetail.Type,
                ServiceType = _appDetail.ServiceType,
                Identity = _appDetail.Identity,
                Name = _appDetail.Name,
                Description = _appDetail.Description,
                SwaggerUrl = _appDetail.SwaggerUrl,
                Url = _appDetail.Url,
                EnvironmentClusterIds = _appDetail.EnvironmentClusters.Select(envCluster=>envCluster.EnvironmentCluster.Id).ToList()
            });
        }

        private void ShowAppModal(UpdateAppDto? model = null)
        {
            _projectEnvClusters = allEnvClusters.Where(envCluster => _projectDetail.EnvironmentClusterIds.Contains(envCluster.Id)).ToList();
            if (model == null)
            {
                _appFormModel.Show();
            }
            else
            {
                _appFormModel.Show(model);
            }
        }

        private async Task SubmitAppAsync()
        {
            _appFormModel.Data.ProjectId = _selectProjectId;
            if (!_appFormModel.HasValue)
            {
                _appFormModel.Data.Type = (AppTypes)_selectAppType;
                _appFormModel.Data.ServiceType = (ServiceTypes)_selectAppServiceType;
                await AppCaller.AddAsync(_appFormModel.Data);
            }
            else
            {
                await AppCaller.UpdateAsync(_appFormModel.Data);
            }

            _apps = await AppCaller.GetListByProjectIdAsync(new List<int> { _selectProjectId });
            _projectApps = _apps.Where(app => app.ProjectId == _selectProjectId).ToList();
            AppHide();
        }

        private async Task RemoveAppAsync()
        {
            var deleteApp = _apps.First(app => app.Id == _selectAppId);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteApp.Name}]应用吗？", async (c) =>
            {
                await AppCaller.DeleteAsync(new RemoveAppDto { AppId = _selectAppId, ProjectId = _selectProjectId });

                _apps.Remove(deleteApp);
                _projectApps.Remove(deleteApp);

                AppHide();
            });
        }

        private void AppHide()
        {
            _selectAppType = 0;
            _selectAppServiceType = 0;
            _appFormModel.Hide();
        }
    }
}
