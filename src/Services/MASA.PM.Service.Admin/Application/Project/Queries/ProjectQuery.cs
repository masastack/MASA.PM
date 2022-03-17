namespace MASA.PM.Service.Admin.Application.Project.Queries
{
    public record ProjectQuery : Query<ProjectDetailDto>
    {
        public override ProjectDetailDto Result { get; set; } = new ProjectDetailDto();

        public int ProjectId { get; set; }
    }
}
