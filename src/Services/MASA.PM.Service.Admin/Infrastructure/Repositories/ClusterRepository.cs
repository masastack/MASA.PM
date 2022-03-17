namespace MASA.PM.Service.Admin.Infrastructure.Repositories
{
    public class ClusterRepository : IClusterRepository
    {
        private readonly PMDbContext _dbContext;

        public ClusterRepository(PMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cluster> AddAsync(Cluster cluster)
        {
            if (_dbContext.Clusters.Any(e => e.Name.ToLower() == cluster.Name.ToLower()))
            {
                throw new Exception("集群名称已存在！");
            }

            await _dbContext.Clusters.AddAsync(cluster);
            await _dbContext.SaveChangesAsync();

            return cluster;
        }

        public async Task AddEnvironmentClusters(IEnumerable<EnvironmentCluster> environmentClusters)
        {
            if (environmentClusters.Any())
            {
                await _dbContext.EnvironmentClusters.AddRangeAsync(environmentClusters);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Cluster> GetAsync(int Id)
        {
            var result = await _dbContext.Clusters.FirstOrDefaultAsync(e => e.Id == Id);

            if (result == null)
            {
                throw new Exception("集群不存在！");
            }

            return result;
        }

        public Task<IQueryable<Cluster>> GetListAsync()
        {
            var result = _dbContext.Clusters.AsQueryable();

            return Task.FromResult(result);
        }

        public async Task<List<EnvironmentCluster>> GetEnvironmentClustersByEnvIdAsync(int envId)
        {
            var result = await _dbContext.EnvironmentClusters.Where(environmentCluster => environmentCluster.EnvironmentId == envId).ToListAsync();

            return result;
        }

        public async Task<List<EnvironmentCluster>> GetEnvironmentClustersByClusterIdAsync(int clusterId)
        {
            var result = await _dbContext.EnvironmentClusters.Where(environmentCluster => environmentCluster.ClusterId == clusterId).ToListAsync();

            return result;
        }

        public async Task<List<EnvironmentCluster>> GetEnvironmentClustersByClusterIdAndEnvironmentIdsAsync(int clusterId, IEnumerable<int> environmentIds)
        {
            var result = await _dbContext.EnvironmentClusters.Where(
                environmentCluster =>
                    environmentCluster.ClusterId == clusterId &&
                    environmentIds.Contains(environmentCluster.EnvironmentId)
                ).ToListAsync();

            return result;
        }

        public async Task<List<EnvironmentCluster>> GetEnvironmentClustersByIds(IEnumerable<int> environmentClusterIds)
        {
            var result = await _dbContext.EnvironmentClusters.Where(envCluster => environmentClusterIds.Contains(envCluster.Id)).ToListAsync();

            return result;
        }

        public async Task<List<(int EnvClusterId, string EnvName, string ClusterName)>> GetEnvironmentClusters()
        {
            var result = await (from envCluster in _dbContext.EnvironmentClusters
                                join env in _dbContext.Environments on envCluster.EnvironmentId equals env.Id
                                join cluster in _dbContext.Clusters on envCluster.ClusterId equals cluster.Id
                                select new ValueTuple<int, string, string>(envCluster.Id, env.Name, cluster.Name)
                                ).ToListAsync();

            return result;
        }

        public async Task UpdateAsync(Cluster cluster)
        {
            if (_dbContext.Clusters.Any(e => e.Name.ToLower() == cluster.Name.ToLower() && e.Id != cluster.Id))
            {
                throw new Exception("集群名称已存在！");
            }

            _dbContext.Clusters.Update(cluster);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEnvironmentClusters(List<EnvironmentCluster> environmentClusters)
        {
            if (environmentClusters.Count > 0)
            {
                _dbContext.EnvironmentClusters.UpdateRange(environmentClusters);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int Id)
        {
            var cluster = await _dbContext.Clusters.FirstOrDefaultAsync(c => c.Id == Id);
            if (cluster == null)
            {
                throw new Exception("集群不存在！");
            }

            _dbContext.Clusters.Remove(cluster);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEnvironmentClusters(List<EnvironmentCluster> environmentClusters)
        {
            if (environmentClusters.Count > 0)
            {
                _dbContext.EnvironmentClusters.RemoveRange(environmentClusters);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteEnvironmentClusterProjects(List<EnvironmentClusterProject> environmentClusterProjects)
        {
            if (environmentClusterProjects.Count > 0)
            {
                _dbContext.EnvironmentClusterProjects.RemoveRange(environmentClusterProjects);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
