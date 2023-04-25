// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class AppModal
    {
        public int ProjectId { get; set; }

        public int EnvironmentClusterId { get; set; }

        public AppDto AppDetail { get; set; } = new();

        [Parameter]
        public EventCallback OnSubmitProjectAfter { get; set; }

        [Inject]
        public ClusterCaller ClusterCaller { get; set; } = default!;

        [Inject]
        public ProjectCaller ProjectCaller { get; set; } = default!;

        [Inject]
        public AppCaller AppCaller { get; set; } = default!;

        private readonly DataModal<UpdateAppDto> _appFormModel = new();
        private List<EnvironmentClusterDto> _projectEnvClusters = new();
        private MForm? _form;

        public async Task InitDataAsync(UpdateAppDto? updateAppDto = null)
        {
            _form?.Reset();
            _appFormModel.Data = new();
            var allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            var projectDetail = await ProjectCaller.GetAsync(ProjectId);
            _projectEnvClusters = allEnvClusters.Where(envCluster => projectDetail.EnvironmentClusterIds.Contains(envCluster.Id)).ToList();

            _appFormModel.Data.ProjectId = ProjectId;

            if (updateAppDto == null)
            {
                _appFormModel.Data.Type = AppTypes.UI;
                _appFormModel.Data.EnvironmentClusterInfos = _projectEnvClusters.Select(p => new EnvironmentClusterInfo(p.Id)).ToList();
                _appFormModel.Show();
            }
            else
            {
                updateAppDto.ProjectId = ProjectId;
                _appFormModel.Show(updateAppDto);
            }

            StateHasChanged();
        }

        private void AppModalValueChanged(bool value)
        {
            _appFormModel.Visible = value;
            if (!value)
            {
                AppDetail = new();
                _appFormModel.Hide();
            }
        }

        private void AppTypeValueChanged(AppTypes appType)
        {
            _appFormModel.Data.Type = appType;
            if (appType == AppTypes.Service)
            {
                _appFormModel.Data.ServiceType = ServiceTypes.Dapr;
            }
        }

        private async Task RemoveAppAsync()
        {
            var result = await PopupService.SimpleConfirmAsync(
                $"{T("Delete app")}",
                $"{T("Are you sure you want to delete AppName \"{AppName}\"?").Replace("{AppName}", AppDetail.Name)}",
                AlertTypes.Error);

            if (result)
            {
                await AppCaller.RemoveAsync(AppDetail.Id);
                await PopupService.EnqueueSnackbarAsync(T("Delete succeeded"), AlertTypes.Success);

                if (OnSubmitProjectAfter.HasDelegate)
                {
                    await OnSubmitProjectAfter.InvokeAsync();
                }
                _appFormModel.Hide();
            }
        }

        private async Task SubmitAppAsync(FormContext context)
        {
            if (context.Validate())
            {
                foreach (var item in _appFormModel.Data.EnvironmentClusterInfos)
                {
                    Regex regex = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0- 9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$");
                    if ((!string.IsNullOrEmpty(item.Url) && !regex.IsMatch(item.Url))
                        || (!string.IsNullOrEmpty(item.SwaggerUrl) && !regex.IsMatch(item.SwaggerUrl)))
                    {
                        await PopupService.EnqueueSnackbarAsync(T("The Url format is incorrect"), AlertTypes.Error);
                        return;
                    }
                }

                if (!_appFormModel.HasValue)
                {
                    await AppCaller.AddAsync(_appFormModel.Data);
                    await PopupService.EnqueueSnackbarAsync(T("Add succeeded"), AlertTypes.Success);
                }
                else
                {
                    await AppCaller.UpdateAsync(_appFormModel.Data);
                    await PopupService.EnqueueSnackbarAsync(T("Edit succeeded"), AlertTypes.Success);
                }

                if (OnSubmitProjectAfter.HasDelegate)
                {
                    await OnSubmitProjectAfter.InvokeAsync();
                }

                AppModalValueChanged(false);
            }
            else if (!_appFormModel.Data.EnvironmentClusterInfos.Any())
            {
                await PopupService.EnqueueSnackbarAsync(T("Environment/Cluster cannot be empty"), AlertTypes.Error);
            }
        }

        private void OnEnvironmentClusterSelectedItemUpdate(List<int> values)
        {
            if (!_appFormModel.Visible) return;

            var oldValues = _appFormModel.Data.EnvironmentClusterInfos.Select(envCluster => envCluster.EnvironmentClusterId);
            var insertValue = values.Except(oldValues).SingleOrDefault();
            var removeValue = oldValues.Except(values).SingleOrDefault();

            if (insertValue != default)
            {
                _appFormModel.Data.EnvironmentClusterInfos.Add(new EnvironmentClusterInfo(insertValue));
            }

            if (removeValue != default)
            {
                _appFormModel.Data.EnvironmentClusterInfos.RemoveAll(ec => ec.EnvironmentClusterId == removeValue);
            }
        }
    }
}
