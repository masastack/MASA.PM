using MASA.PM.Contracts.Base.Model;
using MASA.PM.UI.Admin.Model;

namespace MASA.PM.UI.Admin.Pages.Home
{
    public partial class Init
    {
        private int _step = 1;
        private int _envRadio = 3;
        private List<AddEnvironmentWhitClustersModel> _addEnvironmentWhitClustersModels = new()
        {
            new AddEnvironmentWhitClustersModel("Development", "开发环境"),
            new AddEnvironmentWhitClustersModel("Staging", "测试环境"),
            new AddEnvironmentWhitClustersModel("Production", "生产环境")
        };
        private List<EnvClusterModel> _customEnv = new()
        {
            new EnvClusterModel(0)
        };

        private void AddEnvComponent(int index)
        {
            var env = _customEnv.FirstOrDefault(e => e.Index == index);
            if (env != null)
            {
                var newIndex = _customEnv.IndexOf(env) + 1;
                _customEnv.Insert(newIndex, new EnvClusterModel(newIndex));
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
