using MASA.PM.Service.Admin.Application.Project.Queries;

namespace MASA.PM.Service.Admin.Application.Project
{
    public class ProjectQueryHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IAppRepository _appRepository;

        public ProjectQueryHandler(IProjectRepository projectRepository, IAppRepository appRepository)
        {
            _projectRepository = projectRepository;
            _appRepository = appRepository;
        }

        [EventHandler]
        public async Task GetProject(ProjectQuery query)
        {
            var projectEntity = await _projectRepository.GetAsync(query.ProjectId);

            query.Result = new ProjectViewModel
            {
                Id = projectEntity.Id,
                Name = projectEntity.Name,
                Modifier = projectEntity.Modifier,
                ModificationTime = projectEntity.ModificationTime
            };
        }

        [EventHandler]
        public async Task GetProjects(ProjectsQuery query)
        {
            var projects = await _projectRepository.GetListByEnvironmentClusterIdAsync(query.EnvironmentClusterId);
            query.Result = projects.Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Modifier = project.Modifier,
                ModificationTime = project.ModificationTime,
            }).ToList();
        }
    }
}
