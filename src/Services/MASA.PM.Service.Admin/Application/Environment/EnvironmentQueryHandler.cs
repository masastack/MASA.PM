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
        public async Task EnvironmentHandleAsync(EnvironmentQuery query)
        {
            var cluster = await _environmentRepository.GetAsync(query.EnvironmentId);
            var envclusters = await _clusterRepository.GetEnvironmentClustersByEnvIdAsync(query.EnvironmentId);

            query.Result = new EnvironmentViewModel
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
        public async Task EnvironmentListHandleAsync(EnvironmentsQuery query)
        {
            var envs = await (await _environmentRepository.GetListAsync()).ToListAsync();
            query.Result = envs.Select(env => new EnvironmentsViewModel
            {
                Id = env.Id,
                Name = env.Name
            }).ToList();
        }
    }
}
