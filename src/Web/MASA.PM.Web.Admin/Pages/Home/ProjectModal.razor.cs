﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System.Text.Json;

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

        [Parameter]
        public List<ProjectDto> Projects { get; set; } = new();

        [Parameter]
        public int EnvironmentClusterId { get; set; }

        [Parameter]
        public bool DisableTeamSelect { get; set; }

        [Parameter]
        public EventCallback OnSubmitProjectAfter { get; set; }

        [Parameter]
        public Guid TeamId { get; set; }

        [Parameter]
        public string Environment { get; set; } = string.Empty;


        private DataModal<UpdateProjectDto> _projectFormModel = new();
        private List<TeamModel> _allTeams = new();
        private List<ProjectTypesDto> _projectTypes = new();
        private List<EnvironmentClusterDto> _allEnvClusters = new();
        private ProjectDetailDto _projectDetail = new();
        private List<int> _disableEnvironmentClusterIds = new();
        private MForm? _form;

        public async Task InitDataAsync(ProjectDetailDto? projectDetailDto = null)
        {
            _allTeams = await AuthClient.TeamService.GetAllAsync(Environment);
            _projectTypes = await ProjectCaller.GetProjectTypesAsync();
            _allEnvClusters = await ClusterCaller.GetEnvironmentClusters();

            if (projectDetailDto == null)
            {
                _disableEnvironmentClusterIds.Clear();
                _projectFormModel.Data.EnvironmentName = Environment;

                if (EnvironmentClusterId != 0)
                    _projectFormModel.Data.EnvironmentClusterIds = new List<int> { EnvironmentClusterId };
                else
                    _projectFormModel.Data.EnvironmentClusterIds = _allEnvClusters.Select(envCluster => envCluster.Id).ToList();

                _projectFormModel.Data.TeamIds = new() { TeamId };

                if (_projectTypes.Any())
                    _projectFormModel.Data.LabelCode = _projectTypes[0].Code;

                _projectFormModel.Show();
            }
            else
            {
                _disableEnvironmentClusterIds = (await AppCaller.GetListByProjectIdAsync(new List<int>() { projectDetailDto.Id }))
                    .SelectMany(app => app.EnvironmentClusters.Select(ec => ec.Id))
                    .Distinct()
                    .ToList();

                _projectDetail = projectDetailDto.DeepClone();

                var teamIds = _projectDetail.EnvironmentProjectTeams.FirstOrDefault(c => c.EnvironmentName == Environment)?.TeamIds ?? new List<Guid>();
                _projectFormModel.Show(new UpdateProjectDto
                {
                    Identity = _projectDetail.Identity,
                    LabelCode = _projectDetail.LabelCode,
                    ProjectId = _projectDetail.Id,
                    Name = _projectDetail.Name,
                    TeamIds = teamIds,
                    Description = _projectDetail.Description,
                    EnvironmentClusterIds = _projectDetail.EnvironmentClusterIds,
                    EnvironmentName = Environment
                });
            }

            StateHasChanged();
        }

        private void ProjectModalValueChanged(bool value)
        {
            _projectFormModel.Visible = value;
            if (!value)
            {
                _form?.Reset();
                _projectFormModel.Hide();
            }
        }

        private async Task RemoveProjectAsync()
        {
            var apps = await AppCaller.GetListByProjectIdAsync(new List<int> { _projectDetail.Id });
            if (apps.Any())
            {
                await PopupService.EnqueueSnackbarAsync(T("There are still applications under the current project, which cannot be deleted"), AlertTypes.Error);
            }
            else
            {
                var deleteProject = Projects.First(project => project.Id == _projectDetail.Id);
                var result = await PopupService.SimpleConfirmAsync(T("Delete project"),
                    T("Are you sure you want to delete project \"{ProjectName}\"?").Replace("{ProjectName}",
                    deleteProject.Name),
                    AlertTypes.Error);

                if (result)
                {
                    await ProjectCaller.DeleteAsync(_projectDetail.Id);
                    await PopupService.EnqueueSnackbarAsync(T("Delete succeeded"), AlertTypes.Success);

                    if (OnSubmitProjectAfter.HasDelegate)
                    {
                        await OnSubmitProjectAfter.InvokeAsync();
                    }
                    _projectFormModel.Hide();
                }
            }
        }

        private async Task SubmitProjectAsync(FormContext context)
        {
            await Console.Out.WriteLineAsync($"Data：{JsonSerializer.Serialize(_projectFormModel)}");
            await Console.Out.WriteLineAsync($"Validate：{context.Validate()}");

            if (context.Validate())
            {
                if (!_projectFormModel.HasValue)
                {
                    await ProjectCaller.AddAsync(_projectFormModel.Data);
                    await PopupService.EnqueueSnackbarAsync(T("Add succeeded"), AlertTypes.Success);
                }
                else
                {
                    await ProjectCaller.UpdateAsync(_projectFormModel.Data);
                    await PopupService.EnqueueSnackbarAsync(T("Edit succeeded"), AlertTypes.Success);
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
