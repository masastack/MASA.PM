using MASA.PM.Service.Admin.Application.Environment.Commands;

namespace MASA.PM.Service.Admin.Application.Environment
{
    public class EnvironmentCommandHandler
    {
        private const string ENV_KEY_PREFIX = "masa.pm.env";

        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IClusterRepository _clusterRepository;
        private readonly IMemoryCacheClient _memoryCacheClient;

        public EnvironmentCommandHandler(IEnvironmentRepository environmentRepository, IClusterRepository clusterRepository, IMemoryCacheClient memoryCacheClient)
        {
            _environmentRepository = environmentRepository;
            _clusterRepository = clusterRepository;
            _memoryCacheClient = memoryCacheClient;
        }

        [EventHandler]
        public async Task AddEnvironmentsAndClusterAsync(InitEnvironmentClusterCommand command)
        {
            var envs = command.InitModel.Environments.Select(e => new Infrastructure.Entities.Environment
            {
                Name = e.Name,
                Description = e.Description,
                Creator = Guid.NewGuid(),
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                Modifier = Guid.NewGuid(),
                IsDeleted = false
            }).ToList();

            var envIds = new List<int>();
            foreach (var env in envs)
            {
                var newEnv = await _environmentRepository.AddAsync(env);
                envIds.Add(newEnv.Id);
            }

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

            //add redis cache
            var dicEnvs = envs.Select(env => new EnvModel(env.Id, env.Name)).ToDictionary(env => { return $"{ENV_KEY_PREFIX}.{env.Id}"; });
            var envIdAndNameMappings = envs.ToDictionary(env => $"{ENV_KEY_PREFIX}.{env.Name}", env => env.Id);
            await _memoryCacheClient.SetListAsync(dicEnvs);
            await _memoryCacheClient.SetListAsync(envIdAndNameMappings);
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

            command.Result = new EnvironmentDto { Id = newEnv.Id, Name = newEnv.Name };

            //add redis cache
            await _memoryCacheClient.SetAsync($"{ENV_KEY_PREFIX}.{newEnv.Id}", new EnvModel(newEnv.Id, newEnv.Name));
            await _memoryCacheClient.SetAsync($"{ENV_KEY_PREFIX}.{newEnv.Name}", newEnv.Id);
        }

        [EventHandler]
        public async Task UpdateEnvironmentAsync(UpdateEnvironmentCommand command)
        {
            var envModel = command.EnvironmentModel;
            await _environmentRepository.UpdateAsync(envModel);

            //update redis
            var oldEnv = await _memoryCacheClient.GetAsync<EnvModel>($"{ENV_KEY_PREFIX}.{envModel.EnvironmentId}");
            if (oldEnv != null)
            {
                await _memoryCacheClient.RemoveAsync<int>($"{ENV_KEY_PREFIX}.{oldEnv.Name}");
            }
            await _memoryCacheClient.SetAsync($"{ENV_KEY_PREFIX}.{envModel.Name}", envModel.EnvironmentId);
            await _memoryCacheClient.SetAsync($"{ENV_KEY_PREFIX}.{envModel.EnvironmentId}", new EnvModel(envModel.EnvironmentId, envModel.Name));
        }

        [EventHandler]
        public async Task RemoveEnvironmentAsync(DeleteEnvironmentCommand command)
        {
            await _environmentRepository.RemoveAsync(command.EnvironmentId);

            //remove redis
            var oldEnv = await _memoryCacheClient.GetAsync<EnvModel>($"{ENV_KEY_PREFIX}.{command.EnvironmentId}");
            if (oldEnv != null)
            {
                await _memoryCacheClient.RemoveAsync<int>($"{ENV_KEY_PREFIX}.{oldEnv.Name}");
            }
            await _memoryCacheClient.RemoveAsync<EnvModel>($"{ENV_KEY_PREFIX}.{command.EnvironmentId}");
        }
    }
}
