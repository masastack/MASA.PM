using MASA.PM.Service.Admin.Application.Project.Queries;

namespace MASA.PM.Service.Admin.Application.Project
{
    public class ProjectQueryHander
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectQueryHander(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
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
            query.Result = (await _projectRepository.GetListByEnvironmentClusterIdAsync(query.EnvironmentClusterId)).Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name= project.Name,
                Modifier = project.Modifier,
                ModificationTime = project.ModificationTime
            }).ToList();
        }
    }
}
