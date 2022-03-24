using MASA.PM.Caller.Callers;

namespace MASA.PM.Web.Admin.Pages.Home
{
    public partial class Index
    {
        [Inject]
        public EnvironmentCaller EnvironmentCaller { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var envs = await EnvironmentCaller.GetListAsync();
                if (envs.Count > 0)
                {
                    Nav.NavigateTo(GlobalVariables.DefaultRoute, true);
                }
                else
                {
                    Nav.NavigateTo("init", true);
                }
            }
        }
    }
}
