// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Init
    {

        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        private EnvironmentModel _customEnv = new();
        private int _step = 1;
        private readonly InitDto _initModel = new();
        private bool _initLoading;
        private readonly List<string> _colors = new()
        {
            "#FF7D00", "#37A7FF", "#FF5252", "#37D7AD", "#FFC46C"
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var environments = new List<EnvModel>
                {
                    new EnvModel(0, "Development", T("Development environment"), _colors[0]),
                    new EnvModel(1, "Staging", T("Simulation environment"), _colors[1]),
                    new EnvModel(2, "Production", T("Production environment"), _colors[2])
                };
                _customEnv = new EnvironmentModel()
                {
                    Environments = new List<EnvModel>
                    {
                        new EnvModel(0, "Development", T("Development environment"), _colors[0]),
                        new EnvModel(1, "Staging", T("Simulation environment"), _colors[1]),
                        new EnvModel(2, "Production", T("Production environment"), _colors[2])
                    }
                };

                _customEnv = new EnvironmentModel();
                if (!string.IsNullOrWhiteSpace(MasaStackConfig.Environment))
                {
                    var defaultEnv = environments
                        .FirstOrDefault(e => e.Name.ToLower() == MasaStackConfig.Environment.ToLower());

                    if (defaultEnv != null)
                    {
                        environments.RemoveAll(e => e.Index == defaultEnv.Index);
                        _customEnv.Environments.Add(new EnvModel(0, defaultEnv.Name, defaultEnv.Description, defaultEnv.Color));
                    }
                    else
                    {
                        _customEnv.Environments.Add(new EnvModel(0, MasaStackConfig.Environment, MasaStackConfig.Environment, _colors[2]));
                    }

                    _customEnv.Environments.AddRange(environments);
                }
                else
                {
                    _customEnv.Environments.AddRange(environments);
                }
                _initModel.ClusterName = MasaStackConfig.Cluster;

                var envs = await EnvironmentCaller.GetListAsync();
                if (envs.Count > 0)
                {
                    NavigationManager.NavigateTo(GlobalVariables.DefaultRoute, true);
                }
            }
        }

        private async void NextStep(FormContext context)
        {
            if (context.Validate())
            {
                foreach (var item in _customEnv.Environments)
                {
                    if (_customEnv.Environments.Count(e => e.Name.Equals(item.Name)) > 1)
                    {
                        await PopupService.EnqueueSnackbarAsync(T("The environment name cannot be duplicate"), AlertTypes.Error);
                        return;
                    }
                }
                _step = 2;
            }

        }

        private async Task InitAsync(FormContext context)
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

                    await PopupService.EnqueueSnackbarAsync(T("Initialization completed!"), AlertTypes.Success);
                    NavigationManager.NavigateTo(GlobalVariables.DefaultRoute, true);
                }
                catch (Exception ex)
                {
                    await PopupService.EnqueueSnackbarAsync(ex.Message);
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
