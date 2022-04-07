using MASA.PM.Caller.Callers;
using MASA.PM.Contracts.Admin.Enum;
using MASA.PM.Contracts.Admin.Dto;
using MASA.PM.Contracts.Admin.Dto;

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

        private StringNumber _selectedEnvId = 0;
        private StringNumber _selectEnvClusterId = 0;
        private int _selectProjectId;
        private int _selectAppId;
        private List<EnvironmentDto> _environments = new();
        private List<ClusterDto> _clusters = new();
        private List<ProjectDto> _projects = new();
        private List<AppDto> _allApps = new();
        private List<AppDto> _apps = new();
        private DataModal<UpdateEnvironmentDto> _envFormModel = new();
        private List<ClusterDto> _allClusters = new();
        private List<EnvironmentDto> _allEnvs = new();
        private List<EnvironmentClusterDto> allEnvClusters = new();
        private List<EnvironmentClusterDto> _projectEnvClusters = new();
        private EnvironmentDetailDto _envDetail = new();
        private DataModal<UpdateClusterDto> _clusterFormModel = new();
        private ClusterDetailDto _clusterDetail = new();
        private DataModal<UpdateProjectDto> _projectFormModel = new();
        private ProjectDetailDto _projectDetail = new();
        private DataModal<UpdateAppDto> _appFormModel = new();
        private AppDto _appDetail = new();
        private int _selectAppType;
        private int _selectAppServiceType;
        private AddRelationAppDto _addRelationAppModel = new();
        private bool _relationAppVisible;
        private List<int> _disableRelationAppEnvCluster = new();
        private List<ProjectTypesDto> _projectTypes = new();

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

        private async Task<List<ClusterDto>> GetClustersByEnvIdAsync(int envId, bool isFetchProjects = true)
        {
            _selectedEnvId = envId;
            _clusters = await ClusterCaller.GetListByEnvIdAsync(envId);
            if (_clusters.Any() && isFetchProjects)
            {
                _selectEnvClusterId = _clusters[0].EnvironmentClusterId;
                _projects = await GetProjectByEnvClusterIdAsync(_clusters[0].EnvironmentClusterId);
            }

            return _clusters;
        }

        private async Task<List<ProjectDto>> GetProjectByEnvClusterIdAsync(int envClusterId)
        {
            _selectEnvClusterId = envClusterId;
            _projects = await ProjectCaller.GetListByEnvIdAsync(envClusterId);
            if (_projects.Any())
            {
                var projectIds = _projects.Select(project => project.Id);
                _apps = await GetAppByProjectIdAsync(projectIds);
            }

            return _projects;
        }

        private async Task<List<AppDto>> GetAppByProjectIdAsync(IEnumerable<int> projectIds)
        {
            var apps = await AppCaller.GetListByProjectIdAsync(projectIds.ToList());

            return apps;
        }

        private async Task<EnvironmentDetailDto> GetEnvAsync(int envId)
        {
            _envDetail = await EnvironmentCaller.GetAsync(envId);
            return _envDetail;
        }

        private async Task<ProjectDetailDto> GetProjectAsync(int projectId)
        {
            _projectDetail = await ProjectCaller.GetAsync(projectId);

            return _projectDetail;
        }

        private async Task UpdateEnvAsync(int envId)
        {
            var env = await GetEnvAsync(envId);
            await ShowEnvModalAsync(new UpdateEnvironmentDto
            {
                ClusterIds = env.ClusterIds,
                Name = env.Name,
                Description = env.Description,
                EnvironmentId = env.Id
            });
        }

        private async Task ShowEnvModalAsync(UpdateEnvironmentDto? model = null)
        {
            if (model == null)
            {
                _envFormModel.Show();
            }
            else
            {
                _envFormModel.Show(model);
            }

            _allClusters = await ClusterCaller.GetListAsync();
        }

        private async Task SubmitEnvAsync()
        {
            if (!_envFormModel.HasValue)
            {
                var newEnv = await EnvironmentCaller.AddAsync(_envFormModel.Data);
                _environments.Add(newEnv);
            }
            else
            {
                await EnvironmentCaller.UpdateAsync(_envFormModel.Data);
                var env = _environments.First(env => env.Id == _envFormModel.Data.EnvironmentId);
                env.Name = _envFormModel.Data.Name;
            }

            _envFormModel.Hide();
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
                _clusterFormModel.Show();
            }
            else
            {
                _clusterFormModel.Show(model);
            }

            _allEnvs = await EnvironmentCaller.GetListAsync();
        }

        private async Task SubmitClusterAsync()
        {
            int newClusterId;
            if (!_clusterFormModel.HasValue)
            {
                var cluster = await ClusterCaller.AddAsync(_clusterFormModel.Data);
                newClusterId = cluster.Id;
            }
            else
            {
                await ClusterCaller.UpdateAsync(_clusterFormModel.Data);
                newClusterId = _clusterFormModel.Data.ClusterId;
            }

            await GetClustersByEnvIdAsync(_selectedEnvId.AsT1);
            _clusterFormModel.Hide();
        }

        private async Task RemoveClusterAsync()
        {
            if (_clusters.Count <= 1)
            {
                await PopupService.AlertAsync("集群不能为空", AlertTypes.Error);
            }
            else
            {
                var envName = _environments.First(env => env.Id == _selectedEnvId.AsT1).Name;
                var deleteCluster = _clusters.First(c => c.EnvironmentClusterId == _selectEnvClusterId.AsT1);
                await PopupService.ConfirmAsync("提示", $"确定要删除[{envName}]环境下的[{deleteCluster.Name}]集群吗？", async (c) =>
                {
                    await ClusterCaller.DeleteAsync(_selectEnvClusterId.AsT1);

                    _clusters.Remove(deleteCluster);
                    _selectEnvClusterId = _clusters[0].EnvironmentClusterId;
                    await GetProjectByEnvClusterIdAsync(_clusters[0].EnvironmentClusterId);

                    _clusterFormModel.Hide();
                });
            }
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
                Description = project.Description,
                EnvironmentClusterIds = project.EnvironmentClusterIds
            });
        }

        private async Task ShowProjectModalAsync(UpdateProjectDto? model = null)
        {
            _projectTypes = await ProjectCaller.GetProjectTypesAsync();
            allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            if (model == null)
            {
                _projectFormModel.Show();
            }
            else
            {
                _projectFormModel.Show(model);
            }

            //TODO: get team by auth sdk;
            await Task.Delay(0);
        }

        private async Task SubmitProjectAsync()
        {
            if (!_projectFormModel.HasValue)
            {
                await ProjectCaller.AddAsync(_projectFormModel.Data);
            }
            else
            {
                await ProjectCaller.UpdateAsync(_projectFormModel.Data);
            }

            _projects = await ProjectCaller.GetListByEnvIdAsync(_selectEnvClusterId.AsT1);
            _projectFormModel.Hide();
        }

        private async Task RemoveProjectAsync()
        {
            var deleteProject = _projects.First(project => project.Id == _selectProjectId);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteProject.Name}]项目吗？", async (c) =>
            {
                await ProjectCaller.DeleteAsync(_selectProjectId);

                _projects.Remove(deleteProject);

                _projectFormModel.Hide();
            });
        }

        private void ProjectHide()
        {
            _projectFormModel.Hide();
        }

        private async Task UpdateAppAsync(int appId, int projectId)
        {
            _selectAppId = appId;
            _appDetail = _apps.First(app => app.Id == appId);
            _selectAppType = (int)_appDetail.Type;
            _selectAppServiceType = (int)_appDetail.ServiceType;
            await ShowAppModalAsync(projectId, new UpdateAppDto
            {
                Id = _appDetail.Id,
                Type = _appDetail.Type,
                ServiceType = _appDetail.ServiceType,
                Identity = _appDetail.Identity,
                Name = _appDetail.Name,
                Description = _appDetail.Description,
                SwaggerUrl = _appDetail.SwaggerUrl,
                Url = _appDetail.Url,
                EnvironmentClusterIds = _appDetail.EnvironmentClusters.Select(envCluster=>envCluster.EnvironmentCluster.Id).ToList()
            });
        }

        private async Task ShowAppModalAsync(int projectId, UpdateAppDto? model = null)
        {
            allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            _projectDetail = await ProjectCaller.GetAsync(projectId);
            _projectEnvClusters = allEnvClusters.Where(envCluster => _projectDetail.EnvironmentClusterIds.Contains(envCluster.Id)).ToList();
            _appFormModel.Data.EnvironmentClusterIds = new List<int> { _selectEnvClusterId.AsT1 };
            _selectProjectId = projectId;
            _appFormModel.Data.ProjectId = projectId;

            if (model == null)
            {
                _appFormModel.Show();
            }
            else
            {
                model.ProjectId = projectId;
                _appFormModel.Show(model);
            }
        }

        private void AppHide()
        {
            _appFormModel.Hide();
        }

        private async Task SubmitAppAsync()
        {
            if (!_appFormModel.HasValue)
            {
                await AppCaller.AddAsync(_appFormModel.Data);
            }
            else
            {
                await AppCaller.UpdateAsync(_appFormModel.Data);
            }

            _apps = await GetAppByProjectIdAsync(_projects.Select(project => project.Id).ToList());
            AppHide();
        }

        private async Task RemoveAppAsync()
        {
            var deleteApp = _apps.First(app => app.Id == _selectAppId);
            await PopupService.ConfirmAsync("提示", $"确定要删除[{deleteApp.Name}]应用吗？", async (c) =>
            {
                await AppCaller.DeleteAsync(new RemoveAppDto { AppId = _selectAppId, ProjectId = _selectProjectId });

                _apps = await GetAppByProjectIdAsync(_projects.Select(project => project.Id).ToList());
                AppHide();
            });
        }

        private async Task ShowRelationAppModalAsync(int projectId)
        {
            allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            _projectDetail = await ProjectCaller.GetAsync(projectId);
            _projectEnvClusters = allEnvClusters.Where(envCluster => _projectDetail.EnvironmentClusterIds.Contains(envCluster.Id)).ToList();

            _selectProjectId = projectId;
            _allApps = await AppCaller.GetListAsync();
            _relationAppVisible = true;
        }

        private void RelationAppSelectChange(AppDto model)
        {
            _appDetail = _allApps.First(app => app.Id == model.Id);
            _selectAppType = (int)model.Type;
            _selectAppServiceType = (int)model.ServiceType;
            _addRelationAppModel.AppId = model.Id;
            _addRelationAppModel.EnvironmentClusterIds = new List<int> { _selectEnvClusterId.AsT1 };
            _addRelationAppModel.EnvironmentClusterIds.AddRange(_appDetail.EnvironmentClusters.Select(envCluster => envCluster.EnvironmentCluster.Id));
            _disableRelationAppEnvCluster.AddRange(_addRelationAppModel.EnvironmentClusterIds);
            _addRelationAppModel.ProjectId = _selectProjectId;
        }

        private async Task SubmitRelationAppAsync()
        {
            _addRelationAppModel.EnvironmentClusterIds = _addRelationAppModel.EnvironmentClusterIds.Except(_appDetail.EnvironmentClusters.Select(envCluster => envCluster.EnvironmentCluster.Id)).ToList();
            await AppCaller.AddRelationAppAsync(_addRelationAppModel);
            _apps = await GetAppByProjectIdAsync(_projects.Select(project => project.Id).ToList());
            RelationAppHide();
        }

        private void RelationAppHide()
        {
            _disableRelationAppEnvCluster = new();
            _appDetail = new();
            _addRelationAppModel = new();
            _selectAppType = 0;
            _selectAppServiceType = 0;
            _relationAppVisible = false;
        }
    }
}
