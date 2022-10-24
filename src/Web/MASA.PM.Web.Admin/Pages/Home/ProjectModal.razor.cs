// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class ProjectModal
    {
        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public ClusterCaller ClusterCaller { get; set; } = default!;

        [Inject]
        public AppCaller AppCaller { get; set; } = default!;

        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        [Parameter]
        public List<ProjectDto> Projects { get; set; } = new();

        [Parameter]
        public int EnvironmentClusterId { get; set; }

        [Parameter]
        public bool DisableTeamSelect { get; set; }

        [Parameter]
        public EventCallback OnSubmitProjectAfter { get; set; }

        [Parameter]
        public List<int> DisableEnvironmentClusterIds { get; set; } = new();

        [Parameter]
        public Guid TeamId { get; set; }

        private DataModal<UpdateProjectDto> _projectFormModel = new();
        private List<TeamModel> _allTeams = new();
        private List<ProjectTypesDto> _projectTypes = new();
        private List<EnvironmentClusterDto> _allEnvClusters = new();
        private ProjectDetailDto _projectDetail = new();

        public async Task InitDataAsync(ProjectDetailDto? projectDetailDto = null)
        {
            _allTeams = await AuthClient.TeamService.GetAllAsync();
            _projectTypes = await ProjectCaller.GetProjectTypesAsync();
            _allEnvClusters = await ClusterCaller.GetEnvironmentClusters();

            if (projectDetailDto == null)
            {
                DisableEnvironmentClusterIds.Clear();

                if (EnvironmentClusterId != 0)
                    _projectFormModel.Data.EnvironmentClusterIds = new List<int> { EnvironmentClusterId };
                else
                    _projectFormModel.Data.EnvironmentClusterIds = _allEnvClusters.Select(envCluster => envCluster.Id).ToList();

                _projectFormModel.Data.TeamId = TeamId;

                if (_projectTypes.Any())
                    _projectFormModel.Data.LabelCode = _projectTypes[0].Code;

                _projectFormModel.Show();
            }
            else
            {
                _projectDetail = projectDetailDto;
                _projectFormModel.Show(new UpdateProjectDto
                {
                    Identity = _projectDetail.Identity,
                    LabelCode = _projectDetail.LabelCode,
                    ProjectId = _projectDetail.Id,
                    Name = _projectDetail.Name,
                    TeamId = _projectDetail.TeamId,
                    Description = _projectDetail.Description,
                    EnvironmentClusterIds = _projectDetail.EnvironmentClusterIds
                });
            }

            StateHasChanged();
        }

        private void ProjectModalValueChanged(bool value)
        {
            _projectFormModel.Visible = value;
            if (!value)
            {
                _projectFormModel.Data = new();
            }
        }

        private async Task RemoveProjectAsync()
        {
            var apps = await AppCaller.GetListByProjectIdAsync(new List<int> { _projectDetail.Id });
            if (apps.Any())
            {
                await PopupService.AlertAsync(T("There are still applications under the current project, which cannot be deleted"), AlertTypes.Error);
            }
            else
            {
                var deleteProject = Projects.First(project => project.Id == _projectDetail.Id);
                await PopupService.ConfirmAsync(T("Delete project"),
                    T("Are you sure you want to delete project \"{ProjectName}\"?").Replace("{ProjectName}", deleteProject.Name),
                    AlertTypes.Error,
                    async (c) =>
                    {
                        await ProjectCaller.DeleteAsync(_projectDetail.Id);

                        if (OnSubmitProjectAfter.HasDelegate)
                        {
                            await OnSubmitProjectAfter.InvokeAsync();
                        }

                        _projectFormModel.Hide();
                    });
            }
        }

        private async Task SubmitProjectAsync(FormContext context)
        {
            if (context.Validate())
            {
                if (!_projectFormModel.HasValue)
                {
                    await ProjectCaller.AddAsync(_projectFormModel.Data);
                }
                else
                {
                    await ProjectCaller.UpdateAsync(_projectFormModel.Data);
                }

                if (OnSubmitProjectAfter.HasDelegate)
                {
                    await OnSubmitProjectAfter.InvokeAsync();
                }

                ProjectModalValueChanged(false);
            }
        }
    }
}
