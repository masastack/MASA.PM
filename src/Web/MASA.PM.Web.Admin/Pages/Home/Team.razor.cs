﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Team
    {
        [Parameter]
        public string TeamId { get; set; } = default!;

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
        private bool _disableTeamSelect;
        private UserPortraitModel _userInfo = new();
        private ProjectModal? _projectModal;
        private AppModal? _appModal;
        private ProjectList? _projectListComponent;

        protected override async Task OnParametersSetAsync()
        {
            if (TeamId != _userTeam.Id.ToString())
            {
                _curTab = 0;
                _teamDetailDisabled = true;
                await InitDataAsync();
            }
        }

        private async Task InitDataAsync()
        {
            _userTeam = await AuthClient.TeamService.GetDetailAsync(Guid.Parse(TeamId)) ?? new();
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

        private async Task SearchProject(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && _projectListComponent != null)
            {
                await _projectListComponent.SearchProjectsByNameAsync(_projectName);
            }
        }

        private async Task<ProjectDetailDto> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);

            _userInfo = await GetUserAsync(_projectDetail.Creator);
            _projectDetail.CreatorName = _userInfo.Name;
            _projectDetail.ModifierName = (await GetUserAsync(_projectDetail.Modifier)).Name;
            var team = await AuthClient.TeamService.GetDetailAsync(_projectDetail.TeamId);
            if (team != null)
            {
                _projectDetail.TeamAvatar = team.Avatar;
                _projectDetail.TeamName = team.Name;
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

        private void SearchApp(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
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
        }

        private async Task<List<AppDto>> GetAppByProjectIdsAsync()
        {
            _projectApps = await AppCaller.GetListByProjectIdAsync(new List<int> { _selectProjectId });
            _backupProjectApps = new List<AppDto>(_projectApps.ToArray());

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
                EnvironmentClusterInfo = _appDetail.EnvironmentClusters.Select(envCluster => new ValueTuple<int, string, string>(envCluster.Id, "", "")).ToList()
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
