// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Init
    {
        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        private EnvironmentModel _customEnv = new();
        private int _step = 1;
        private InitDto _initModel = new();
        private bool _initLoading;
        private readonly List<string> _colors = new()
        {
            "success", "warning", "error", "info", "orange lighten-1"
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var defaultColor = _colors.First();
                _customEnv = new EnvironmentModel()
                {
                    Environments = new List<EnvModel>
                    {
                        new EnvModel(0, "Development", "开发环境", defaultColor),
                        new EnvModel(1, "Staging", "模拟环境", defaultColor),
                        new EnvModel(2, "Production", "生产环境", defaultColor)
                    }
                };

                var envs = await EnvironmentCaller.GetListAsync();
                if (envs.Count > 0)
                {
                    NavigationManager.NavigateTo(GlobalVariables.DefaultRoute, true);
                }
            }
        }

        private async void NextStep(EditContext context)
        {
            if (context.Validate())
            {
                foreach (var item in _customEnv.Environments)
                {
                    if (_customEnv.Environments.Count(e => e.Name.Equals(item.Name)) > 1)
                    {
                        await PopupService.ToastErrorAsync("环境名称不允许重复");
                        return;
                    }
                }
                _step = 2;
            }

        }

        private async Task InitAsync(EditContext context)
        {
            if (context.Validate())
            {
                _initLoading = true;

                try
                {
                    _initModel.Environments = _customEnv.Environments.Select(env => new AddEnvironmentDto
                    {
                        Name = env.Name,
                        Description = env.Description,
                        Color = env.Color
                    }).ToList();
                    await EnvironmentCaller.InitAsync(_initModel);

                    await PopupService.AlertAsync("初始化完成！", AlertTypes.Success);
                    NavigationManager.NavigateTo(GlobalVariables.DefaultRoute, true);
                }
                catch (Exception ex)
                {
                    await PopupService.AlertAsync(ex);
                }
                finally
                {
                    _initLoading = false;
                }
            }
        }

        private void AddEnvComponent(int index)
        {
            var env = _customEnv.Environments.FirstOrDefault(e => e.Index == index);
            if (env != null)
            {
                var newIndex = _customEnv.Environments.IndexOf(env) + 1;
                _customEnv.Environments.Insert(newIndex, new EnvModel(_customEnv.Environments.Count));
            }
        }

        private void RemoveEnvComponent(int index)
        {
            if (_customEnv.Environments.Count > 1)
            {
                var env = _customEnv.Environments.FirstOrDefault(e => e.Index == index);
                if (env != null)
                {
                    _customEnv.Environments.Remove(env);
                }
            }
        }
    }
}
