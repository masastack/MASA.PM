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

        private StringNumber _curTab = 0;
        private bool _teamDetailDisabled = true;
        private List<ProjectDto> _projects = new();
        private List<AppDto> _apps = new();
        private string _projectName = "";
        private ProjectDetailDto _projectDetail = new();
        private List<EnvironmentClusterDto> allEnvClusters = new();
        private int _selectProjectId;
        private int _appCount;
        private List<AppDto> _projectApps = new();
        private string _appName = "";
        private AppDto _appDetail = new();
        private List<TeamModel> _allTeams = new();
        private List<TeamModel> _userTeams = new();
        private TeamModel _userTeam = new();
        private ProjectModal? _projectModal;
        private AppModal? _appModal;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _allTeams = await AuthClient.TeamService.GetUserTeamsAsync();
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
            if (args.Key == "Enter")
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
                LabelCode = project.LabelCode,
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

        private async Task GetProjectDetailAsync(int projectId)
        {
            _teamDetailDisabled = false;
            _curTab = 1;

            allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            _selectProjectId = projectId;
            _projectApps = _apps.Where(app => app.ProjectId == projectId).ToList();
            _appCount = _projectApps.Count;
            await GetProjectAsync(projectId);
        }

        private void SearchApp(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
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
        }

        private async Task<List<AppDto>> GetAppByProjectIdsAsync(IEnumerable<int>? projectIds = null)
        {
            var newProjectIds = projectIds ?? _projects.Select(p => p.Id);

            _apps = await AppCaller.GetListByProjectIdAsync(newProjectIds.ToList());

            return _apps;
        }

        private async Task UpdateAppAsync(AppDto app)
        {
            _appDetail = app;

            if (_appModal != null)
            {
                _appDetail.CreatorName = (await GetUserAsync(_appDetail.Creator)).Name;
                _appDetail.ModifierName = (await GetUserAsync(_appDetail.Modifier)).Name;
                _appModal.AppDetail = _appDetail;
            }

            await ShowAppModalAsync(app.ProjectId, new UpdateAppDto
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

        private async Task ShowAppModalAsync(int projectId, UpdateAppDto? model = null)
        {
            if (_appModal != null)
            {
                _appModal.ProjectId = projectId;
                await _appModal.InitDataAsync(model);
            }
        }
    }
}
