using MASA.PM.Service.Admin.Application.Project.Commands;

namespace MASA.PM.Service.Admin.Application.Project
{
    public class ProjectCommandHandler
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [EventHandler]
        public async Task AddProjectAsync(AddProjectCommand command)
        {
            var project = new Infrastructure.Entities.Project
            {
                Identity = command.ProjectModel.Identity,
                TypeId = command.ProjectModel.TypeId,
                Name = command.ProjectModel.Name,
                Description = command.ProjectModel.Description,
                TeamId = command.ProjectModel.TeamId,
            };
            var newPeoject = await _projectRepository.AddAsync(project);

            var environmentClusterProjects = command.ProjectModel.EnvironmentClusterIds.Select(environmentClusterId => new EnvironmentClusterProject
            {
                EnvironmentClusterId = environmentClusterId,
                ProjectId = newPeoject.Id
            });

            await _projectRepository.AddEnvironmentClusterProjectsAsync(environmentClusterProjects);
        }

        [EventHandler]
        public async Task UpdateProjectAsync(UpdateProjectCommand command)
        {
            var project = new Infrastructure.Entities.Project
            {
                Id = command.ProjectModel.ProjectId,
                Name = command.ProjectModel.Name,
                Description = command.ProjectModel.Description,
                TeamId = command.ProjectModel.TeamId,
                Modifier = MasaUser.UserId
            };

            await _projectRepository.UpdateAsync(project);

            var oldEnvironmentClusterIds = (
                    await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectModel.ProjectId)
                )
                .Select(environmentClusterProject => environmentClusterProject.EnvironmentClusterId)
                .ToList();

            //need to delete EnvironmentClusterProject
            var deleteEnvironmentClusterIds = oldEnvironmentClusterIds.Except(command.ProjectModel.EnvironmentClusterIds);
            if (deleteEnvironmentClusterIds.Any())
            {
                var deleteEnvironmentClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(command.ProjectModel.ProjectId, deleteEnvironmentClusterIds);
                
                await _projectRepository.RemoveEnvironmentClusterProjects(deleteEnvironmentClusterProjects);
            }

            //need to add EnvironmentClusterProject
            var addEnvironmentClusterIds = command.ProjectModel.EnvironmentClusterIds.Except(oldEnvironmentClusterIds).ToList();
            if (addEnvironmentClusterIds.Any())
            {
                await _projectRepository.IsExistedProjectName(command.ProjectModel.Name, addEnvironmentClusterIds);

                await _projectRepository.AddEnvironmentClusterProjectsAsync(addEnvironmentClusterIds.Select(environmentClusterId => new EnvironmentClusterProject
                {
                    EnvironmentClusterId = environmentClusterId,
                    ProjectId = command.ProjectModel.ProjectId
                }));
            }
        }

        [EventHandler]
        public async Task DeleteProjectAsync(DeleteProjectCommand command)
        {
            await _projectRepository.RemoveAsync(command.ProjectId);

            var environmentClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectId);
            await _projectRepository.RemoveEnvironmentClusterProjects(environmentClusterProjects);
        }
    }
}
