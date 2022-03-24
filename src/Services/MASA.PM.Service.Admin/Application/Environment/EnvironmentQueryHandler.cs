namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentQueryHandler
    {
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IClusterRepository _clusterRepository;

        public EnvironmentQueryHandler(IEnvironmentRepository environmentRepository, IClusterRepository clusterRepository)
        {
            _environmentRepository = environmentRepository;
            _clusterRepository = clusterRepository;
        }

        [EventHandler]
        public async Task GetEnvironmentAsync(EnvironmentQuery query)
        {
            var cluster = await _environmentRepository.GetAsync(query.EnvironmentId);
            var envclusters = await _clusterRepository.GetEnvironmentClustersByEnvIdAsync(query.EnvironmentId);

            query.Result = new EnvironmentDetailDto
            {
                Id = cluster.Id,
                Name = cluster.Name,
                Description = cluster.Description,
                Creator = cluster.Creator,
                CreationTime = cluster.CreationTime,
                Modifier = cluster.Modifier,
                ModificationTime = cluster.ModificationTime,
                ClusterIds = envclusters.Select(ec => ec.ClusterId).ToList()
            };
        }

        [EventHandler]
        public async Task GetEnvironmentListAsync(EnvironmentsQuery query)
        {
            var envs = await (await _environmentRepository.GetListAsync()).OrderByDescending(env => env.ModificationTime).ToListAsync();
            query.Result = envs.Select(env => new EnvironmentDto
            {
                Id = env.Id,
                Name = env.Name
            }).ToList();
        }
    }
}
