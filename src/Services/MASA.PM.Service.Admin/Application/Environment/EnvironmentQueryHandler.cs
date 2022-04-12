namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentQueryHandler
    {
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IClusterRepository _clusterRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAppRepository _appRepository;

        public EnvironmentQueryHandler(IEnvironmentRepository environmentRepository, IClusterRepository clusterRepository, IProjectRepository projectRepository, IAppRepository appRepository)
        {
            _environmentRepository = environmentRepository;
            _clusterRepository = clusterRepository;
            _projectRepository = projectRepository;
            _appRepository = appRepository;
        }

        [EventHandler]
        public async Task GetEnvironmentAsync(EnvironmentQuery query)
        {
            var environment = await _environmentRepository.GetAsync(query.EnvironmentId);
            var envclusters = await _clusterRepository.GetEnvironmentClustersByEnvIdAsync(query.EnvironmentId);

            query.Result = new EnvironmentDetailDto
            {
                Id = environment.Id,
                Name = environment.Name,
                Color = environment.Color,
                Description = environment.Description,
                Creator = environment.Creator,
                CreationTime = environment.CreationTime,
                Modifier = environment.Modifier,
                ModificationTime = environment.ModificationTime,
                ClusterIds = envclusters.Select(ec => ec.ClusterId).ToList()
            };
        }

        [EventHandler]
        public async Task GetEnvironmentListAsync(EnvironmentsQuery query)
        {
            var envs = await _environmentRepository.GetListAsync();
            query.Result = envs.Select(env => new EnvironmentDto
            {
                Id = env.Id,
                Name = env.Name,
                Color = env.Color,
            }).ToList();
        }
    }
}
