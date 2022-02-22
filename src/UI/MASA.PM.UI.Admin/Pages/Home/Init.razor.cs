using MASA.Blazor.Experimental.Components;
using MASA.PM.Caller.Environment;
using MASA.PM.Contracts.Base.Model;
using MASA.PM.UI.Admin.Model;

namespace MASA.PM.UI.Admin.Pages.Home
{
    public partial class Init
    {
        private readonly List<EnvClusterModel> _customEnv = new()
        {
            new EnvClusterModel(0)
        };
        private int _step = 1;
        private int _envRadio = 3;
        private InitModel _initModel = new();
        private readonly Func<string, StringBoolean> _requiredRule = value => !string.IsNullOrEmpty(value) ? true : "Required.";
        
        private IEnumerable<Func<string, StringBoolean>> RequiredRules => new List<Func<string, StringBoolean>>
        {
            _requiredRule
        };

        [Inject]
        public IPopupService PopupService { get; set; } = default!;

        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        private async Task InitAsync()
        {
            if (_envRadio == 0)
            {
                _initModel.Environments = _customEnv.Select(env => new AddEnvironmentModel { Name = env.Name, Description = env.Description }).ToList();
            }

            await EnvironmentCaller.InitProjectAsync(_initModel);

            await PopupService.MessageAsync("初始化完成！", AlertTypes.Success);
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
