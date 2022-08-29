﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Landscape
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

        private int _projectCount;
        private StringNumber _selectedEnvId = 0;
        private StringNumber _selectEnvClusterId = 0;
        private List<EnvironmentDto> _environments = new();
        private List<ClusterDto> _clusters = new();
        private List<ProjectDto> _projects = new();
        private readonly DataModal<UpdateEnvironmentDto> _envFormModel = new();
        private List<ClusterDto> _allClusters = new();
        private List<EnvironmentDto> _allEnvs = new();
        private EnvironmentDetailDto _envDetail = new();
        private readonly DataModal<UpdateClusterDto> _clusterFormModel = new();
        private ClusterDetailDto _clusterDetail = new();
        private readonly List<string> _colors = new()
        {
            "#FF7D00", "#37A7FF", "#FF5252", "#37D7AD", "#FFC46C",
        };
        private ProjectList? _projectListComponent;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _environments = await EnvironmentCaller.GetListAsync();
                if (_environments.Any())
                {
                    _selectedEnvId = _environments[0].Id;
                    _clusters = await GetClustersByEnvIdAsync(_environments[0].Id);
                }
                else
                {
                    NavigationManager.NavigateTo("init", true);
                }

                StateHasChanged();
            }
        }

        private async Task<List<ClusterDto>> GetClustersByEnvIdAsync(int envId)
        {
            _selectedEnvId = envId;
            _clusters = await ClusterCaller.GetListByEnvIdAsync(envId);

            if (_clusters.Any())
            {
                _selectEnvClusterId = _clusters[0].EnvironmentClusterId;
            }
            else
            {
                _selectEnvClusterId = 0;
            }

            return _clusters;
        }

        private async Task<EnvironmentDetailDto> GetEnvAsync(int envId)
        {
            _envDetail = await EnvironmentCaller.GetAsync(envId);
            return _envDetail;
        }

        private async Task UpdateEnvAsync(int envId)
        {
            var env = await GetEnvAsync(envId);
            await ShowEnvModalAsync(new UpdateEnvironmentDto
            {
                ClusterIds = env.ClusterIds,
                Name = env.Name,
                Description = env.Description,
                EnvironmentId = env.Id,
                Color = env.Color
            });
        }

        private async Task ShowEnvModalAsync(UpdateEnvironmentDto? model = null)
        {
            if (model == null)
            {
                _envFormModel.Data.Color = _colors.First();
                _envFormModel.Show();
            }
            else
            {
                _envDetail.ModifierName = (await GetUserAsync(_envDetail.Modifier)).Name;
                _envDetail.CreatorName = (await GetUserAsync(_envDetail.Creator)).Name;
                _envFormModel.Show(model);
            }

            _allClusters = await ClusterCaller.GetListAsync();
        }

        private async Task SubmitEnvAsync(EditContext context)
        {
            if (context.Validate())
            {
                if (!_envFormModel.HasValue)
                {
                    var newEnv = await EnvironmentCaller.AddAsync(_envFormModel.Data);
                    _environments.Add(newEnv);
                }
                else
                {
                    await EnvironmentCaller.UpdateAsync(_envFormModel.Data);

                    var newEnv = await EnvironmentCaller.GetAsync(_envFormModel.Data.EnvironmentId);
                    var env = _environments.First(e => e.Id == newEnv.Id);
                    env.Name = newEnv.Name;
                    env.Color = newEnv.Color;
                    await GetClustersByEnvIdAsync(newEnv.Id);
                }

                EnvModalValueChanged(false);
            }
        }

        private async Task RemoveEnvAsync()
        {
            if (_environments.Count <= 1)
            {
                await PopupService.AlertAsync("环境不能为空", AlertTypes.Error);
            }
            else
            {
                var envId = _selectedEnvId.AsT1;
                var deleteEnv = _environments.First(c => c.Id == envId);

                await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteEnv.Name}]环境吗？", async (c) =>
                {
                    await EnvironmentCaller.DeleteAsync(envId);

                    _environments.Remove(deleteEnv);
                    _selectedEnvId = _environments[0].Id;
                    await GetClustersByEnvIdAsync(_environments[0].Id);

                    _envFormModel.Hide();
                });
            }
        }

        private void EnvModalValueChanged(bool value)
        {
            _envFormModel.Visible = value;
            if (!value)
            {
                _envFormModel.Data = new();
            }
        }

        private async Task<ClusterDetailDto> GetClusterAsync(int clusterId)
        {
            _clusterDetail = await ClusterCaller.GetAsync(clusterId);

            return _clusterDetail;
        }

        private async Task UpdateClusterAsync(int clusterId)
        {
            var cluster = await GetClusterAsync(clusterId);
            await ShowClusterModalAsync(new UpdateClusterDto
            {
                ClusterId = cluster.Id,
                Name = cluster.Name,
                Description = cluster.Description,
                EnvironmentIds = cluster.EnvironmentIds
            });

        }

        private async Task ShowClusterModalAsync(UpdateClusterDto? model = null)
        {
            if (model == null)
            {
                _clusterFormModel.Data.EnvironmentIds.Add(_selectedEnvId.AsT1);
                _clusterFormModel.Show();
            }
            else
            {
                _clusterDetail.ModifierName = (await GetUserAsync(_clusterDetail.Modifier)).Name;
                _clusterDetail.CreatorName = (await GetUserAsync(_clusterDetail.Creator)).Name;
                _clusterFormModel.Show(model);
            }

            _allEnvs = await EnvironmentCaller.GetListAsync();
        }

        private async Task SubmitClusterAsync(EditContext context)
        {
            if (context.Validate())
            {
                if (!_clusterFormModel.HasValue)
                {
                    await ClusterCaller.AddAsync(_clusterFormModel.Data);
                }
                else
                {
                    await ClusterCaller.UpdateAsync(_clusterFormModel.Data);
                }

                await GetClustersByEnvIdAsync(_selectedEnvId.AsT1);

                ClusterModalValueChanged(false);
            }
        }

        private async Task RemoveClusterAsync()
        {
            if (_clusters.Count <= 1)
            {
                await PopupService.AlertAsync("集群不能为空", AlertTypes.Error);
            }
            else
            {
                var deleteCluster = _clusters.First(c => c.EnvironmentClusterId == _selectEnvClusterId.AsT1);
                await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteCluster.Name}]集群吗？", async (c) =>
                {
                    try
                    {
                        await ClusterCaller.RemoveAsync(deleteCluster.Id);
                    }
                    catch (Exception ex)
                    {
                        c.Cancel();
                        await PopupService.ToastErrorAsync(ex.Message);
                    }

                    _clusters.Remove(deleteCluster);
                    _selectEnvClusterId = _clusters[0].EnvironmentClusterId;
                    _clusterFormModel.Hide();
                });
            }
        }

        private void ClusterModalValueChanged(bool value)
        {
            _clusterFormModel.Visible = value;
            if (!value)
            {
                _clusterFormModel.Data = new();
            }
        }

        private async Task ShowProjectModalAsync(ProjectDetailDto? model = null)
        {
            if (_projectListComponent != null)
            {
                await _projectListComponent.ShowProjectModalAsync(model);
            }
        }

        private void GetProjectCount(int projectCount)
        {
            _projectCount = projectCount;
        }
    }
}
