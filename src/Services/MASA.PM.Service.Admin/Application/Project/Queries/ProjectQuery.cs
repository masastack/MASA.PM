namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectQuery : Query<ProjectViewModel>
    {
        public override ProjectViewModel Result { get; set; } = new ProjectViewModel();

        public int ProjectId { get; set; }
    }
}
