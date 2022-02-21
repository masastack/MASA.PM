using MASA.PM.Service.Admin.Application.Environment.Commands;

namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentCommandHander
    {
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IClusterRepository _clusterRepository;

        public EnvironmentCommandHander(IEnvironmentRepository environmentRepository, IClusterRepository clusterRepository)
        {
            _environmentRepository = environmentRepository;
            _clusterRepository = clusterRepository;
        }

        [EventHandler]
        public async Task AddEnvironmentsAndClusterAsync(InitEnvironmentClusterCommand command)
        {
            var envs = command.InitModel.Environments.Select(e => new Infrastructure.Entities.Environment
            {
                Name = e.Name,
                Description = e.Description
            }).ToList();

            var envIds = await _environmentRepository.AddEnvironmentsAndClusterAsync(envs);
            var cluster = await _clusterRepository.AddAsync(new Infrastructure.Entities.Cluster
            {
                Name = command.InitModel.ClusterName,
                Description = command.InitModel.ClusterDescription
            });

            var envClusters = envIds.Select(envId => new EnvironmentCluster
            {
                EnvironmentId = envId,
                ClusterId = cluster.Id,
            });

            await _environmentRepository.AddEnvironmentClustersAsync(envClusters);
        }

        [EventHandler]
        public async Task AddEnvironmentWithClustersAsync(AddEnvironmentCommand command)
        {
            var addEnvEntity = new Infrastructure.Entities.Environment
            {
                Name = command.EnvironmentWhitClusterModel.Name,
                Description = command.EnvironmentWhitClusterModel.Description
            };
            var newEnv = await _environmentRepository.AddAsync(addEnvEntity);

            var addEnvironmentClusters = new List<EnvironmentCluster>();
            command.EnvironmentWhitClusterModel.ClusterIds.ForEach(clusterId =>
            {
                addEnvironmentClusters.Add(new EnvironmentCluster
                {
                    ClusterId = clusterId,
                    EnvironmentId = newEnv.Id
                });
            });
            await _environmentRepository.AddEnvironmentClustersAsync(addEnvironmentClusters);
        }

        [EventHandler]
        public async Task UpdateEnvironmentAsync(UpdateEnvironmentCommand command)
        {
            await _environmentRepository.UpdateAsync(command.EnvironmentModel);
        }

        [EventHandler]
        public async Task DeleteEnvironmentAsync(DeleteEnvironmentCommand command)
        {
            await _environmentRepository.DeleteAsync(command.EnvironmentId);
        }
    }
}
