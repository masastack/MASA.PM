using MASA.PM.Service.Admin.Application.Project.Commands;

namespace MASA.PM.Service.Admin.Application.Project
{
    public class ProjectCommandHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IMemoryCacheClient _memoryCacheClient;
        private const string PROJECT_KEYS = "masa.pm.project.keys";
        private const string PROJECT_KEY_PREFIX = "masa.pm.project.key";

        public ProjectCommandHandler(IProjectRepository projectRepository, IEnvironmentRepository environmentRepository, IMemoryCacheClient memoryCacheClient)
        {
            _projectRepository = projectRepository;
            _environmentRepository = environmentRepository;
            _memoryCacheClient = memoryCacheClient;
        }

        [EventHandler]
        public async Task AddProjectAsync(AddProjectCommand command)
        {
            var project = new Infrastructure.Entities.Project
            {
                Identity = command.ProjectModel.Identity,
                LabelId = command.ProjectModel.LabelId,
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

            //add redis cache
            var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(command.ProjectModel.EnvironmentClusterIds);
            foreach (var env in envs)
            {
                List<string> keys = await _memoryCacheClient.GetAsync<List<string>?>($"{PROJECT_KEYS}.{env.Name.ToLower()}") ?? new List<string>();
                string projectKey = $"{PROJECT_KEY_PREFIX}.{env.Name.ToLower()}.{project.Id}";
                keys.Add(projectKey);
                await _memoryCacheClient.SetAsync<List<string>>($"{PROJECT_KEYS}.{env.Name.ToLower()}", keys);
                await _memoryCacheClient.SetAsync<Infrastructure.Entities.Project>(projectKey, project);
            }
        }

        [EventHandler]
        public async Task UpdateProjectAsync(UpdateProjectCommand command)
        {
            var project = await _projectRepository.GetAsync(command.ProjectModel.ProjectId);

            project.Name = command.ProjectModel.Name;
            project.Description = command.ProjectModel.Description;
            project.TeamId = command.ProjectModel.TeamId;
            project.Modifier = MasaUser.UserId;
            project.ModificationTime = DateTime.Now;


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

                //remove redis cache
                var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(deleteEnvironmentClusterProjects.Select(e => e.EnvironmentClusterId));
                foreach (var env in envs)
                {
                    string projectKey = $"{PROJECT_KEY_PREFIX}.{env.Name.ToLower()}.{command.ProjectModel.ProjectId}";
                    await _memoryCacheClient.RemoveAsync<Infrastructure.Entities.Project>(projectKey);
                }
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

                //add redis cache
                var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(command.ProjectModel.EnvironmentClusterIds);
                foreach (var env in envs)
                {
                    List<string> keys = await _memoryCacheClient.GetAsync<List<string>?>($"{PROJECT_KEYS}.{env.Name.ToLower()}") ?? new List<string>();
                    string projectKey = $"{PROJECT_KEY_PREFIX}.{env.Name.ToLower()}.{project.Id}";
                    keys.Add(projectKey);
                    await _memoryCacheClient.SetAsync<List<string>>($"{PROJECT_KEYS}.{env.Name.ToLower()}", keys);
                    await _memoryCacheClient.SetAsync<Infrastructure.Entities.Project>(projectKey, project);
                }
            }
        }

        [EventHandler]
        public async Task RemoveProjectAsync(DeleteProjectCommand command)
        {
            await _projectRepository.RemoveAsync(command.ProjectId);

            var environmentClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectId);
            await _projectRepository.RemoveEnvironmentClusterProjects(environmentClusterProjects);

            var envs = await _environmentRepository.GetListByEnvClusterIdsAsync(environmentClusterProjects.Select(e => e.EnvironmentClusterId));
            foreach (var env in envs)
            {
                string projectKey = $"{PROJECT_KEY_PREFIX}.{env.Name.ToLower()}.{command.ProjectId}";
                await _memoryCacheClient.RemoveAsync<Infrastructure.Entities.Project>(projectKey);
            }
        }
    }
}
