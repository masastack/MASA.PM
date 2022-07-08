// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Team
    {
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

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IAuthClient AuthClient { get; set; } = default!;

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
        private int _selectAppId;
        private List<TeamModel> _userTeams = new();
        private ProjectModal? _projectModal;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _userTeams = await AuthClient.TeamService.GetUserTeamsAsync();
                var envs = await EnvironmentCaller.GetListAsync();
                if (envs.Count <= 0)
                {
                    NavigationManager.NavigateTo("init", true);
                }

                await InitDataAsync();
                StateHasChanged();
            }
        }

        private async Task InitDataAsync()
        {
            _projects = await GetProjectListAsync();
            var projectIds = _projects.Select(project => project.Id).ToList();
            _apps = await AppCaller.GetListByProjectIdAsync(projectIds);
        }

        private async Task<List<ProjectDto>> GetProjectListAsync()
        {
            _projects = await ProjectCaller.GetListByTeamIdsAsync(_userTeams.Select(t => t.Id));

            return _projects;
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
            await ShowProjectModalAsync(new UpdateProjectDto
            {
                Identity = project.Identity,
                LabelId = project.LabelId,
                ProjectId = project.Id,
                Name = project.Name,
                TeamId = project.TeamId,
                Description = project.Description,
                EnvironmentClusterIds = project.EnvironmentClusterIds
            });
        }

        private async Task ShowProjectModalAsync(UpdateProjectDto? model = null)
        {
            if (_projectModal != null)
            {
                await _projectModal.InitDataAsync(model);
            }
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
                EnvironmentClusterIds = _appDetail.EnvironmentClusters.Select(envCluster => envCluster.Id).ToList()
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

        private async Task SubmitAppAsync(EditContext context)
        {
            if (context.Validate())
            {
                _appFormModel.Data.ProjectId = _selectProjectId;
                if (!_appFormModel.HasValue)
                {
                    await AppCaller.AddAsync(_appFormModel.Data);
                }
                else
                {
                    if (!_appFormModel.Data.EnvironmentClusterIds.Any())
                    {
                        await PopupService.ToastErrorAsync("环境/集群不能为空");
                        return;
                    }

                    await AppCaller.UpdateAsync(_appFormModel.Data);
                }

                _apps = await AppCaller.GetListByProjectIdAsync(new List<int> { _selectProjectId });
                _projectApps = _apps.Where(app => app.ProjectId == _selectProjectId).ToList();
                AppValueChanged(false);
            }
        }

        private async Task RemoveAppAsync()
        {
            var deleteApp = _apps.First(app => app.Id == _selectAppId);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteApp.Name}]应用吗？", async (c) =>
            {
                await AppCaller.RemoveAsync(_selectAppId);

                _apps.Remove(deleteApp);
                _projectApps.Remove(deleteApp);

                _appCount = _projectApps.Count;

                AppValueChanged(false);
            });
        }

        private void AppValueChanged(bool value)
        {
            _appFormModel.Visible = value;
            if (!value)
            {
                _appFormModel.Hide();
            }
        }
    }
}
