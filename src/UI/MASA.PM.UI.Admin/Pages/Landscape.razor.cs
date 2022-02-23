using MASA.PM.Caller.Environment;
using MASA.PM.Contracts.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.UI.Admin.Pages
{
    public partial class Landscape
    {
        private int _selectedEnvId;
        private List<EnvironmentsViewModel> _environments = new();

        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _environments =await EnvironmentCaller.GetListAsync();

                StateHasChanged();
            }
        }

        private async Task ChangeEnv(int id, bool firstRender = false)
        {
            
        }
    }
}
