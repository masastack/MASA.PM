namespace MASA.PM.Service.Admin.Infrastructure.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private readonly PMDbContext _dbContext;

        public EnvironmentRepository(PMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<int>> AddEnvironmentsAndClusterAsync(List<Entities.Environment> environments)
        {
            await Task.Delay(1);
            List<int> envIds = new();

            if (environments.Any())
            {
                environments.ForEach(env =>
                {
                    var newEnv = _dbContext.Environments.Add(env);
                    _dbContext.SaveChanges();
                    envIds.Add(newEnv.Entity.Id);
                });
            }

            return envIds;
        }

        public async Task<Entities.Environment> AddAsync(Entities.Environment environment)
        {
            if (_dbContext.Environments.Any(e => e.Name.ToLower() == environment.Name.ToLower()))
            {
                throw new Exception("环境名称已存在！");
            }

            await _dbContext.Environments.AddAsync(environment);
            await _dbContext.SaveChangesAsync();

            return environment;
        }

        public async Task AddEnvironmentClustersAsync(IEnumerable<EnvironmentCluster> environmentClusters)
        {
            if (environmentClusters.Any())
            {
                await _dbContext.EnvironmentClusters.AddRangeAsync(environmentClusters);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<IQueryable<Entities.Environment>> GetListAsync()
        {
            var result = _dbContext.Environments.AsQueryable();

            return Task.FromResult(result);
        }

        public async Task<Entities.Environment> GetAsync(int Id)
        {
            var result = await _dbContext.Environments.FirstOrDefaultAsync(env => env.Id == Id);

            if (result == null)
            {
                throw new Exception("环境不存在！");
            }

            return result;
        }

        public async Task UpdateAsync(UpdateEnvironmentModel model)
        {
            if (_dbContext.Environments.Any(e => e.Name.ToLower() == model.Name.ToLower() && e.Id != model.EnvironmentId))
            {
                throw new Exception("环境名称已存在！");
            }

            _dbContext.Environments.Update(new Entities.Environment
            {
                Id = model.EnvironmentId,
                Name = model.Name,
                Description = model.Description,
                Modifier = model.ActionUserId
            });

            var oldClusterIds = await _dbContext.EnvironmentClusters.Where(e => e.EnvironmentId == model.EnvironmentId).Select(e => e.ClusterId).ToListAsync();

            // EnvironmentClusters need to delete
            var deleteClusterIds = oldClusterIds.Except(model.ClusterIds);
            if (deleteClusterIds.Any())
            {
                var deleteEnvironmentClusters = await _dbContext.EnvironmentClusters.Where(e => e.EnvironmentId == model.EnvironmentId && deleteClusterIds.Contains(e.ClusterId)).ToListAsync();
                _dbContext.EnvironmentClusters.RemoveRange(deleteEnvironmentClusters);
            }

            // EnvironmentClusters need to insert
            var addClusterIds = model.ClusterIds.Except(oldClusterIds);
            if (addClusterIds.Any())
            {
                _dbContext.EnvironmentClusters.AddRange(addClusterIds.Select(clusterId => new EnvironmentCluster
                {
                    EnvironmentId = model.EnvironmentId,
                    ClusterId = clusterId
                }));
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var environment = await _dbContext.Environments.FirstOrDefaultAsync(env => env.Id == Id);
            if (environment == null)
            {
                throw new Exception("环境不存在！");
            }
            //var environmentClusters = await _dbContext.EnvironmentClusters.Where(e => e.EnvironmentId == environment.Id).ToListAsync();
            //var environmentClusterIds = environmentClusters.Select(e => e.Id);
            //var environmentClusterProjects = await _dbContext.EnvironmentClusterProjects.Where(e => environmentClusterIds.Contains(e.EnvironmentClusterId)).ToListAsync();

            _dbContext.Environments.Remove(environment);
            //_dbContext.EnvironmentClusters.RemoveRange(environmentClusters);
            //_dbContext.EnvironmentClusterProjects.RemoveRange(environmentClusterProjects);

            await _dbContext.SaveChangesAsync();
        }
    }
}
