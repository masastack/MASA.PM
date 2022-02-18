using MASA.PM.Service.Admin.Application.Cluster.Commands;

namespace MASA.PM.Service.Admin.Application.Cluster
{
    public class ClusterCommandHander
    {
        private readonly IClusterRepository _clusterRepository;

        public ClusterCommandHander(IClusterRepository clusterRepository)
        {
            _clusterRepository = clusterRepository;
        }

        [EventHandler]
        public async Task AddEnvironmentWithClustersAsync(AddClusterCommand command)
        {
            var addClusterEntity = new Infrastructure.Entities.Cluster
            {
                Name = command.ClustersWhitEnvironmentModel.Name,
                Description = command.ClustersWhitEnvironmentModel.Description
            };
            var newCluster = await _clusterRepository.AddAsync(addClusterEntity);

            var addEnvironmentClusters = new List<EnvironmentCluster>();
            command.ClustersWhitEnvironmentModel.EnvironmentIds.ForEach(environmentId =>
            {
                addEnvironmentClusters.Add(new EnvironmentCluster
                {
                    ClusterId = newCluster.Id,
                    EnvironmentId = environmentId
                });
            });
            await _clusterRepository.AddEnvironmentClusters(addEnvironmentClusters);
        }

        [EventHandler]
        public async Task UpdateClusterAsync(UpdateClusterCommand command)
        {
            var updateClusterModel = command.UpdateClusterModel;
            await _clusterRepository.UpdateAsync(new Infrastructure.Entities.Cluster
            {
                Id = updateClusterModel.ClusterId,
                Name = updateClusterModel.Name,
                Description = updateClusterModel.Description,
                Modifier = updateClusterModel.ActionUserId
            });

            var oldEnvironmentIds = (
                    await _clusterRepository.GetEnvironmentClustersByClusterIdAsync(updateClusterModel.ClusterId)
                )
                .Select(environmentCluster => environmentCluster.EnvironmentId)
                .ToList();

            // EnvironmentClusters need to delete
            var deleteEnvironmentIds = oldEnvironmentIds.Except(updateClusterModel.EnvironmentIds);
            if (deleteEnvironmentIds.Any())
            {
                var deleteEnvironmentClusters = await _clusterRepository.GetEnvironmentClustersByClusterIdAndEnvironmentIdsAsync(updateClusterModel.ClusterId, deleteEnvironmentIds);
                await _clusterRepository.DeleteEnvironmentClusters(deleteEnvironmentClusters);
            }

            // EnvironmentClusters need to insert
            var addEnvironmentIds = updateClusterModel.EnvironmentIds.Except(oldEnvironmentIds);
            if (addEnvironmentIds.Any())
            {
                await _clusterRepository.AddEnvironmentClusters(addEnvironmentIds.Select(environmentId => new EnvironmentCluster
                {
                    EnvironmentId = environmentId,
                    ClusterId = updateClusterModel.ClusterId,
                    Creator = updateClusterModel.ActionUserId,
                    Modifier = updateClusterModel.ActionUserId
                }));
            }
        }

        [EventHandler]
        public async Task DeleteClusterAsync(DeleteClusterCommand command)
        {
            await _clusterRepository.DeleteAsync(command.ClusterId);

            var environmentClusters = await _clusterRepository.GetEnvironmentClustersByClusterIdAsync(command.ClusterId);
            await _clusterRepository.DeleteEnvironmentClusters(environmentClusters);

            var environmentClusterProjects = await _clusterRepository.GetEnvironmentClusterProjectsByClusterIdAsync(environmentClusters.Select(ec => ec.Id));
            await _clusterRepository.DeleteEnvironmentClusterProjects(environmentClusterProjects);
        }
    }
}
