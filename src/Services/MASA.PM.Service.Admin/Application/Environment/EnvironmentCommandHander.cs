using MASA.PM.Service.Admin.Application.Environment.Commands;

namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentCommandHander
    {
        private readonly IEnvironmentRepository _environmentRepository;

        public EnvironmentCommandHander(IEnvironmentRepository environmentRepository)
        {
            _environmentRepository = environmentRepository;
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
