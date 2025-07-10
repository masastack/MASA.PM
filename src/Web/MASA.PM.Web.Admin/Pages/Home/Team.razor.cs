// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.BuildingBlocks.Authentication.Identity;

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Team
    {
        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        [Inject]
        public ClusterCaller ClusterCaller { get; set; } = default!;

        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public AppCaller AppCaller { get; set; } = default!;

        [Inject]
        public GlobalConfig GlobalConfig { get; set; } = default!;

        [Inject]
        public IMultiEnvironmentUserContext MultiEnvironmentUserContext { get; set; } = default!;

        private int _projectCount;
        private StringNumber _curTab = 0;
        private bool _teamDetailDisabled = true;
        private string _projectName = "";
        private ProjectDetailDto _projectDetail = new();
        private List<EnvironmentClusterDto> _allEnvClusters = new();
        private int _selectProjectId;
        private int _appCount;
        private List<AppDto> _projectApps = new();
        private List<AppDto> _backupProjectApps = new();
        private string _appName = "";
        private AppDto _appDetail = new();
        private TeamDetailModel _userTeam = new();
        private UserModel _userInfo = new();
        private bool _disableTeamSelect;
        private ProjectModal? _projectModal;
        private AppModal? _appModal;
        private ProjectList? _projectListComponent;
        private Guid _teamId;
        private List<TeamDetailModel> _teams = new();
        private Dictionary<int, List<UserModel>> _appUsers = new();

        protected override Task OnInitializedAsync()
        {
            GlobalConfig.OnCurrentTeamChanged += HandleCurrentTeamChanged;
            _users = new();
            return base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var teamId = GlobalConfig.CurrentTeamId == Guid.Empty ? MasaUser.CurrentTeamId : GlobalConfig.CurrentTeamId;
                HandleCurrentTeamChanged(teamId);
            }
        }

        private async void HandleCurrentTeamChanged(Guid teamId)
        {
            if (teamId != _userTeam.Id)
            {
                _teamId = teamId;
                await TabItemChangedAsync(0);
                _teamDetailDisabled = true;
                await InitDataAsync(teamId);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task InitDataAsync(Guid teamId)
        {
            _userTeam = await AuthClient.TeamService.GetDetailAsync(teamId) ?? new();
            var envs = await EnvironmentCaller.GetListAsync();
            if (envs.Count <= 0)
            {
                NavigationManager.NavigateTo("init", true);
            }
        }

        private void GetProjectCount(int projectCount)
        {
            _projectCount = projectCount;
        }

        private async Task OnSubmitProjectAsyncAfter()
        {
            if (_curTab == 1)
            {
                await GetProjectDetailAsync(_selectProjectId);
            }
        }

        private async Task TabItemChangedAsync(StringNumber value)
        {
            _curTab = value;
            if (value == 0)
            {
                _projectListComponent?.InitDataAsync();
            }
            else if (value == 1)
            {
                await GetProjectDetailAsync(_selectProjectId);
            }
        }

        private async Task SearchProject()
        {
            if (_projectListComponent != null)
            {
                await _projectListComponent.SearchProjectsByNameAsync(_projectName);
            }
        }

        private async Task<ProjectDetailDto> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);
            _userInfo = await GetUserAsync(_projectDetail.Creator);
            _projectDetail.CreatorName = _userInfo.RealDisplayName;
            _projectDetail.ModifierName = (await GetUserAsync(_projectDetail.Modifier)).RealDisplayName;
            _appUsers = await LoadResponsibilityUsersAsync(_projectApps);
            var teamIds = _projectDetail.EnvironmentProjectTeams.FirstOrDefault(c => c.EnvironmentName == MultiEnvironmentUserContext.Environment)?.TeamIds ?? [];
            if (teamIds.Count > 0)
            {
                foreach (var teamId in teamIds)
                {
                    if (teamId == Guid.Empty) continue;
                    var team = await AuthClient.TeamService.GetDetailAsync(teamId);
                    if (team == null) continue;
                    _teams.Add(team);
                }
            }
            return _projectDetail;
        }

        private async Task UpdateProjectAsync(int projectId, bool disableTeamSelect = false)
        {
            _disableTeamSelect = disableTeamSelect;
            _selectProjectId = projectId;
            await GetProjectAsync(projectId);
            if (_projectModal != null)
            {
                await _projectModal.InitDataAsync(_projectDetail);
            }
        }

        private async Task ShowProjectModalAsync(ProjectDetailDto? model = null)
        {
            if (_projectListComponent != null)
            {
                await _projectListComponent.ShowProjectModalAsync(model);
            }
        }

        private async Task GetProjectDetailAsync(int projectId)
        {
            _teamDetailDisabled = false;
            _curTab = 1;

            _selectProjectId = projectId;
            await GetAppByProjectIdsAsync();
            _allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            _appCount = _projectApps.Count;
            await GetProjectAsync(projectId);
        }

        private void SearchApp()
        {
            if (!string.IsNullOrWhiteSpace(_appName))
            {
                _projectApps = _backupProjectApps.Where(app => app.Name.ToLower().Contains(_appName.ToLower())).ToList();
            }
            else
            {
                _projectApps = _backupProjectApps;
            }
        }

        private async Task<List<AppDto>> GetAppByProjectIdsAsync()
        {
            _projectApps = await AppCaller.GetListByProjectIdAsync(new List<int> { _selectProjectId });

            foreach (var app in _projectApps)
            {
                app.ModifierName = (await GetUserAsync(app.Modifier)).RealDisplayName;
            }
            _backupProjectApps = new List<AppDto>(_projectApps.ToArray());
            _appUsers = await LoadResponsibilityUsersAsync(_projectApps);
            return _projectApps;
        }

        private async Task OnSubmitAppAsyncAfter()
        {
            await GetAppByProjectIdsAsync();
            if (_curTab == 1)
            {
                await GetProjectDetailAsync(_selectProjectId);
            }
        }

        private async Task UpdateAppAsync(AppDto app)
        {
            _appDetail = app;

            if (_appModal != null)
            {
                _appDetail.CreatorName = (await GetUserAsync(_appDetail.Creator)).RealDisplayName;
                _appDetail.ModifierName = (await GetUserAsync(_appDetail.Modifier)).RealDisplayName;
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
                ResponsibilityUsers = _appDetail.ResponsibilityUserIds!,
                EnvironmentClusterInfos = _appDetail.EnvironmentClusters.Select(envCluster => new EnvironmentClusterInfo(envCluster.Id, envCluster.AppURL, envCluster.AppSwaggerURL)).ToList()
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

        public void Dispose()
        {
            GlobalConfig.OnCurrentTeamChanged -= HandleCurrentTeamChanged;
        }
    }
}
