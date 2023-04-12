// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class ProjectList
    {
        [Parameter]
        public EventCallback<int> OnNameClick { get; set; }

        [Parameter]
        public bool CanAddApp { get; set; }

        [Parameter]
        public int EnvironmentClusterId { get; set; }

        [Parameter]
        public Guid TeamId { get; set; }

        [Parameter]
        public EventCallback<int> FetchProjectCount { get; set; }

        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public AppCaller AppCaller { get; set; } = default!;

        private int _internalEnvironmentClusterId;
        private Guid _internalTeamId;
        public List<ProjectDto> _projects = new();
        private List<ProjectDto> _backupProjects = new();
        public List<TeamModel> _allTeams = new();
        private List<AppDto> _apps = new();
        private ProjectDetailDto _projectDetail = new();
        private AppDto _appDetail = new();
        private bool _showProcess = false;
        private ProjectModal? _projectModal;
        private AppModal? _appModal;

        protected override async Task OnParametersSetAsync()
        {
            if (EnvironmentClusterId != _internalEnvironmentClusterId || TeamId != _internalTeamId)
            {
                _showProcess = true;
                _internalEnvironmentClusterId = EnvironmentClusterId;
                _internalTeamId = TeamId;

                try
                {
                    await InitDataAsync();

                    if (FetchProjectCount.HasDelegate)
                    {
                        await FetchProjectCount.InvokeAsync(_projects.Count);
                    }
                }
                finally
                {
                    _showProcess = false;
                }
            }
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }

        public async Task ShowProjectModalAsync(ProjectDetailDto? model = null)
        {
            if (_projectModal != null)
            {
                await _projectModal.InitDataAsync(model);
            }
        }

        public async Task SearchProjectsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                await InitDataAsync();
            }
            else
            {
                _projects = _backupProjects.Where(project => project.Name.ToLower().Contains(name.ToLower())).ToList();
            }
        }

        public async Task InitDataAsync()
        {
            if (CanAddApp)
            {
                if (EnvironmentClusterId == 0)
                {
                    _projects.Clear();
                    _backupProjects.Clear();
                }
                else
                {
                    _projects = await ProjectCaller.GetListByEnvClusterIdAsync(EnvironmentClusterId);
                }
            }
            else
            {
                _projects = await ProjectCaller.GetListByTeamIdsAsync(new List<Guid> { TeamId });
            }

            _projects.ForEach(async project =>
            {
                project.ModifierName = (await GetUserAsync(project.Modifier)).StaffDislpayName;
            });

            _allTeams = await AuthClient.TeamService.GetAllAsync();
            _backupProjects = new List<ProjectDto>(_projects.ToArray());
            _apps = await AppCaller.GetListByProjectIdAsync(_projects.Select(p => p.Id).ToList());
        }

        private async Task UpdateProjectAsync(int projectId)
        {
            var project = await GetProjectAsync(projectId);

            await ShowProjectModalAsync(project);
        }

        private async Task<ProjectDetailDto> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);

            _projectDetail.CreatorName = (await GetUserAsync(_projectDetail.Creator)).StaffDislpayName;
            _projectDetail.ModifierName = (await GetUserAsync(_projectDetail.Modifier)).StaffDislpayName;

            return _projectDetail;
        }

        private async Task UpdateAppAsync(AppDto app)
        {
            _appDetail = app;

            if (_appModal != null)
            {
                _appDetail.CreatorName = (await GetUserAsync(_appDetail.Creator)).StaffDislpayName;
                _appDetail.ModifierName = (await GetUserAsync(_appDetail.Modifier)).StaffDislpayName;
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
                EnvironmentClusterInfos = _appDetail.EnvironmentClusters.Select(envCluster =>
                {
                    return new EnvironmentClusterInfo(envCluster.Id, envCluster.AppURL, envCluster.AppSwaggerURL);
                }).ToList()
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

        private async Task<List<AppDto>> GetAppByProjectIdsAsync(IEnumerable<int> projectIds)
        {
            _apps = await AppCaller.GetListByProjectIdAsync(projectIds.ToList());

            return _apps;
        }

        private async Task OnSubmitProjectAsyncAfter()
        {
            if (EnvironmentClusterId != 0)
            {
                _projects = await ProjectCaller.GetListByEnvClusterIdAsync(EnvironmentClusterId);
            }
            else
            {
                _projects = await ProjectCaller.GetListByTeamIdsAsync(new List<Guid> { TeamId });
            }
        }

        private async Task OnSubmitAppAsyncAfter()
        {
            await GetAppByProjectIdsAsync(_projects.Select(project => project.Id));
        }

        private async Task HandleProjectNameClick(int projectId)
        {
            await OnNameClick.InvokeAsync(projectId);
        }
    }
}
