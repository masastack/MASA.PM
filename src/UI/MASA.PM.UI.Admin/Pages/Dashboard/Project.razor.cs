using MASA.PM.Caller.Callers;
using MASA.PM.Contracts.Base.ViewModel;

namespace MASA.PM.UI.Admin.Pages.Dashboard
{
    public partial class Project
    {
        private string? _search;

        [Parameter]
        public List<ProjectsViewModel> Projects { get; set; } = new();

        [Parameter]
        public List<AppViewModel> Apps { get; set; } = new();

        [Parameter]
        public string Keyword { get; set; } = "";

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        private async Task OnKeyDownHandle(KeyboardEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(Keyword) && OnKeyDown.HasDelegate)
            {
                await OnKeyDown.InvokeAsync();
            }
        }
    }
}
