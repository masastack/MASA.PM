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

        public Func<Task<List<ProjectDto>>> ProjectDataSource = () => { return Task.FromResult(new List<ProjectDto>()); };

        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public AppCaller AppCaller { get; set; } = default!;

        private List<ProjectDto> _projects = new();
        private List<AppDto> _apps = new();
        private ProjectDetailDto _projectDetail = new();
        private AppDto _appDetail = new();
        private UserPortraitModel _userInfo = new();
        private TeamDetailModel _userTeam = new();
        private ProjectModal? _projectModal;
        private AppModal? _appModal;

        public void ClearProjects()
        {
            _projects.Clear();
            StateHasChanged();
        }

        public void SetApps(List<AppDto> app)
        {
            _apps = app;
        }

        private async Task UpdateProjectAsync(int projectId)
        {
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

        private async Task<ProjectDetailDto> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);

            _userInfo = await GetUserAsync(_projectDetail.Creator);
            _projectDetail.CreatorName = _userInfo.Name;
            _projectDetail.ModifierName = (await GetUserAsync(_projectDetail.Modifier)).Name;

            return _projectDetail;
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

        public async Task<List<ProjectDto>> GetProjectListAsync()
        {
            _projects = await ProjectDataSource.Invoke();
            StateHasChanged();
            return _projects;
        }

        private async Task<List<AppDto>> GetAppByProjectIdsAsync(IEnumerable<int>? projectIds = null)
        {
            var newProjectIds = projectIds ?? _projects.Select(p => p.Id);

            _apps = await AppCaller.GetListByProjectIdAsync(newProjectIds.ToList());

            return _apps;
        }

        private async Task OnSubmitProjectAsyncAfter()
        {
            await GetProjectListAsync();
        }

        private async Task OnSubmitAppAsyncAfter()
        {
            await GetAppByProjectIdsAsync();
        }

        private async Task HandleProjectNameClick(int projectId)
        {
            await OnNameClick.InvokeAsync(projectId);
        }
    }
}
