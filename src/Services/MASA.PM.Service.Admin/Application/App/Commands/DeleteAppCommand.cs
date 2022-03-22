namespace MASA.PM.Service.Admin.Application.App.Commands
{
    public record DeleteAppCommand(int AppId,int ProjectId) : Command
    {
    }
}
