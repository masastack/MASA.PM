// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class ProjectModal
    {
        [Inject]
        public IAuthClient AuthClient { get; set; } = default!;

        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public ClusterCaller ClusterCaller { get; set; } = default!;

        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        [Parameter]
        public List<ProjectDto> Projects { get; set; } = new();

        [Parameter]
        public EventCallback OnSubmitProjectAfter { get; set; }

        private DataModal<UpdateProjectDto> _projectFormModel = new();
        private List<TeamModel> _allTeams = new();
        private List<ProjectTypesDto> _projectTypes = new();
        private List<EnvironmentClusterDto> _allEnvClusters = new();
        private ProjectDetailDto _projectDetail = new();

        public async Task InitDataAsync(UpdateProjectDto? updateProjectDto = null)
        {
            if (updateProjectDto == null)
            {
                _projectFormModel.Show();
            }
            else
            {
                _projectFormModel.Show(updateProjectDto);
                await GetProjectAsync(updateProjectDto.ProjectId);
            }

            _allTeams = await AuthClient.TeamService.GetAllAsync();
            _projectTypes = await ProjectCaller.GetProjectTypesAsync();
            _allEnvClusters = await ClusterCaller.GetEnvironmentClusters();

            StateHasChanged();
        }

        public async Task GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);
        }

        private void ProjectModalValueChanged(bool value)
        {
            _projectFormModel.Visible = value;
            if (!value)
            {
                _projectFormModel.Hide();
            }
        }

        private async Task RemoveProjectAsync()
        {
            var deleteProject = Projects.First(project => project.Id == _projectDetail.Id);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteProject.Name}]项目吗？", async (c) =>
            {
                await ProjectCaller.DeleteAsync(_projectDetail.Id);

                if (OnSubmitProjectAfter.HasDelegate)
                {
                    await OnSubmitProjectAfter.InvokeAsync();
                }

                _projectFormModel.Hide();
            });
        }

        private async Task SubmitProjectAsync(EditContext context)
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
                _projectFormModel.Hide();
            }
        }
    }
}
