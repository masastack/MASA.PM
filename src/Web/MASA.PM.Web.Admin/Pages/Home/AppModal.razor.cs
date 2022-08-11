﻿// Copyright (c) MASA Stack All rights reserved.
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

        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        private readonly DataModal<UpdateAppDto> _appFormModel = new();
        private List<EnvironmentClusterDto> _projectEnvClusters = new();

        public async Task InitDataAsync(UpdateAppDto? updateAppDto = null)
        {
            var allEnvClusters = await ClusterCaller.GetEnvironmentClusters();
            var projectDetail = await ProjectCaller.GetAsync(ProjectId);
            _projectEnvClusters = allEnvClusters.Where(envCluster => projectDetail.EnvironmentClusterIds.Contains(envCluster.Id)).ToList();

            _appFormModel.Data.ProjectId = ProjectId;
            _appFormModel.Data.EnvironmentClusterIds = new List<int> { EnvironmentClusterId };

            if (updateAppDto == null)
            {
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
                _appFormModel.Data = new();
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
            await PopupService.ConfirmAsync("提示", $"确定要删除[{AppDetail.Name}]应用吗？", async (c) =>
            {
                await AppCaller.RemoveAsync(AppDetail.Id);

                if (OnSubmitProjectAfter.HasDelegate)
                {
                    await OnSubmitProjectAfter.InvokeAsync();
                }
                _appFormModel.Hide();
            });
        }

        private async Task SubmitAppAsync(EditContext context)
        {
            if (context.Validate())
            {
                if (!string.IsNullOrEmpty(_appFormModel.Data.Url))
                {
                    Regex urlRegex = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0- 9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$");
                    if (!urlRegex.IsMatch(_appFormModel.Data.Url))
                    {
                        await PopupService.ToastErrorAsync(T("The Url format is incorrect"));
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(_appFormModel.Data.SwaggerUrl))
                {
                    Regex regex = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0- 9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$");
                    if (!regex.IsMatch(_appFormModel.Data.SwaggerUrl))
                    {
                        await PopupService.ToastErrorAsync(T("The SwaggerUrl format is incorrect"));
                        return;
                    }
                }

                if (!_appFormModel.HasValue)
                {
                    await AppCaller.AddAsync(_appFormModel.Data);
                }
                else
                {
                    if (!_appFormModel.Data.EnvironmentClusterIds.Any())
                    {
                        await PopupService.ToastErrorAsync(T("Environment/Cluster cannot be empty"));
                        return;
                    }

                    await AppCaller.UpdateAsync(_appFormModel.Data);
                }

                if (OnSubmitProjectAfter.HasDelegate)
                {
                    await OnSubmitProjectAfter.InvokeAsync();
                }

                AppModalValueChanged(false);
            }
        }
    }
}