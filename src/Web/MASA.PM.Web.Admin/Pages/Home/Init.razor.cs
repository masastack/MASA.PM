using MASA.Blazor.Experimental.Components;
using MASA.PM.Caller.Callers;
using MASA.PM.UI.Admin.Model;

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Init
    {
        private readonly List<EnvClusterModel> _customEnv = new()
        {
            new EnvClusterModel(0, "Development", "开发环境"),
            new EnvClusterModel(1, "Staging", "模拟环境"),
            new EnvClusterModel(2, "Production", "生产环境")
        };
        private int _step = 1;
        private InitDto _initModel = new();
        private readonly Func<string, StringBoolean> _requiredRule = value => !string.IsNullOrEmpty(value) ? true : "Required.";
        private bool _initLoading;

        private IEnumerable<Func<string, StringBoolean>> RequiredRules => new List<Func<string, StringBoolean>>
        {
            _requiredRule
        };

        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var envs = await EnvironmentCaller.GetListAsync();
                if (envs.Count > 0)
                {
                    NavigationManager.NavigateTo(GlobalVariables.DefaultRoute, true);
                }
            }
        }

        private async Task InitAsync()
        {
            _initLoading = true;

            try
            {
                await EnvironmentCaller.InitAsync(_initModel);
            }
            catch (Exception ex)
            {
                await PopupService.MessageAsync(ex);
            }
            finally
            {
                _initLoading = false;
            }

            await PopupService.MessageAsync("初始化完成！", AlertTypes.Success);

            NavigationManager.NavigateTo(GlobalVariables.DefaultRoute, true);
        }

        private void AddEnvComponent(int index)
        {
            var env = _customEnv.FirstOrDefault(e => e.Index == index);
            if (env != null)
            {
                var newIndex = _customEnv.IndexOf(env) + 1;
                _customEnv.Insert(newIndex, new EnvClusterModel(_customEnv.Count));
            }
        }

        private void RemoveEnvComponent(int index)
        {
            if (_customEnv.Count > 1)
            {
                var env = _customEnv.FirstOrDefault(e => e.Index == index);
                if (env != null)
                {
                    _customEnv.Remove(env);
                }
            }
        }
    }
}
