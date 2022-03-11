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
            var environmentCluster = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(projectEntity.Id);
            query.Result = new ProjectViewModel
            {
                Id = projectEntity.Id,
                Name = projectEntity.Name,
                Description = projectEntity.Description,
                TeamId = projectEntity.TeamId,
                EnvironmentClusterIds = environmentCluster.Select(envCluster => envCluster.EnvironmentClusterId).ToList(),
                CreationTime = projectEntity.CreationTime,
                Creator = projectEntity.Creator,
                Modifier = projectEntity.Modifier,
                ModificationTime = projectEntity.ModificationTime
            };
        }

        [EventHandler]
        public async Task GetProjects(ProjectsQuery query)
        {
            if (query.EnvironmentClusterId.HasValue)
            {
                var projects = await _projectRepository.GetListByEnvironmentClusterIdAsync(query.EnvironmentClusterId.Value);
                query.Result = projects.Select(project => new ProjectsViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Modifier = project.Modifier,
                    ModificationTime = project.ModificationTime,
                }).ToList();
            }
            else
            {
                var projects = await _projectRepository.GetListAsync();
                query.Result = projects.Select(project => new ProjectsViewModel
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
}
