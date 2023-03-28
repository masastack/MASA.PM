// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private readonly PmDbContext _dbContext;
        private readonly II18n<DefaultResource> _i18N;

        public EnvironmentRepository(PmDbContext dbContext, II18n<DefaultResource> i18N)
        {
            _dbContext = dbContext;
            _i18N = i18N;
        }

        public async Task<Entities.Environment> AddAsync(Entities.Environment environment)
        {
            if (_dbContext.Environments.Any(e => e.Name.ToLower() == environment.Name.ToLower()))
            {
                throw new UserFriendlyException(_i18N.T("Environment name already exists!"));
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

        public async Task<EnvironmentCluster> AddEnvironmentClusterAsync(EnvironmentCluster environmentCluster)
        {
            await _dbContext.AddAsync(environmentCluster);
            await _dbContext.SaveChangesAsync();

            return environmentCluster;
        }

        public async Task<List<Entities.Environment>> GetListAsync()
        {
            var result = await _dbContext.Environments.ToListAsync();

            return result;
        }

        public async Task<Entities.Environment> GetAsync(int Id)
        {
            var result = await _dbContext.Environments.FirstOrDefaultAsync(env => env.Id == Id);

            if (result == null)
            {
                throw new UserFriendlyException(_i18N.T("Environment does not exist!"));
            }

            return result;
        }

        public async Task UpdateAsync(UpdateEnvironmentDto model)
        {
            if (_dbContext.Environments.Any(e => e.Name.ToLower() == model.Name.ToLower() && e.Id != model.EnvironmentId))
            {
                throw new UserFriendlyException(_i18N.T("Environment name already exists!"));
            }

            _dbContext.Environments.Update(new Entities.Environment(model.EnvironmentId, model.Name, model.Description, model.Color));

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

        public async Task RemoveAsync(int Id)
        {
            var envCount = await _dbContext.EnvironmentClusters.CountAsync();
            if (envCount <= 1)
            {
                throw new UserFriendlyException(_i18N.T("Environment cannot be empty!"));
            }

            var environment = await _dbContext.Environments.FirstOrDefaultAsync(env => env.Id == Id);
            if (environment == null)
            {
                throw new UserFriendlyException(_i18N.T("Environment does not exist!"));
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
