namespace MASA.PM.Service.Admin.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PMDbContext _dbContext;

        public ProjectRepository(PMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project> AddAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task AddEnvironmentClusterProjectsAsync(IEnumerable<EnvironmentClusterProject> environmentClusterProjects)
        {
            if (environmentClusterProjects.Any())
            {
                await _dbContext.EnvironmentClusterProjects.AddRangeAsync(environmentClusterProjects);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int Id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(project => project.Id == Id);
            if (project == null)
            {
                throw new Exception("项目不存在！");
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEnvironmentClusterProjects(IEnumerable<EnvironmentClusterProject> environmentClusterProjects)
        {
            if (environmentClusterProjects.Any())
            {
                _dbContext.EnvironmentClusterProjects.RemoveRange(environmentClusterProjects);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Project> GetAsync(int Id)
        {
            var result = await _dbContext.Projects.FirstOrDefaultAsync(project => project.Id == Id);

            return result ?? throw new Exception("项目不存在！");
        }

        public async Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAsync(int projectId)
        {
            var result = await _dbContext.EnvironmentClusterProjects.Where(environmentClusterProject => environmentClusterProject.ProjectId == projectId).ToListAsync();

            return result;
        }

        public async Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(int projectId, IEnumerable<int> environmentClusterIds)
        {
            var result = await _dbContext.EnvironmentClusterProjects.Where(ecp => ecp.ProjectId == projectId && environmentClusterIds.Contains(ecp.EnvironmentClusterId)).ToListAsync();

            return result;
        }

        public async Task<List<int>> GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(IEnumerable<int> envClusterIds, int projectId)
        {
            var result = await _dbContext.EnvironmentClusterProjects.Where(ecp => envClusterIds.Contains(ecp.EnvironmentClusterId) && ecp.ProjectId == projectId)
                .Select(ecp => ecp.Id)
                .ToListAsync();

            return result;
        }

        public async Task<List<Project>> GetListByEnvironmentClusterIdAsync(int environmentClusterId)
        {
            System.Linq.Expressions.Expression<Func<EnvironmentClusterProject, bool>> predicate = environmentClusterProject =>
                                    environmentClusterProject.EnvironmentClusterId == environmentClusterId;

            var projectIds = await _dbContext.EnvironmentClusterProjects.Where(predicate)
                .Select(project => project.ProjectId)
                .ToListAsync();

            var result = await _dbContext.Projects.Where(project => projectIds.Contains(project.Id)).ToListAsync();
            return result;
        }

        public async Task UpdateAsync(Project project)
        {
            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task IsExistedProjectName(string name, List<int> environmentClusterIds, params int[] excludeProjectIds)
        {
            var result = await (from project in _dbContext.Projects.Where(project => project.Name.ToLower() == name.ToLower() && !excludeProjectIds.Contains(project.Id))
                                join ecp in _dbContext.EnvironmentClusterProjects on project.Id equals ecp.ProjectId
                                join ec in _dbContext.EnvironmentClusters.Where(envCluster => environmentClusterIds.Contains(envCluster.Id)) on ecp.EnvironmentClusterId equals ec.Id
                                join e in _dbContext.Environments on ec.EnvironmentId equals e.Id
                                join c in _dbContext.Clusters on ec.ClusterId equals c.Id
                                select new
                                {
                                    EnvironmentName = e.Name,
                                    ClusterName = c.Name
                                }).FirstOrDefaultAsync();

            if (result != null)
            {
                throw new Exception($"项目名[{name}]已在环境[{result.EnvironmentName}]/环境[{result.ClusterName}]中存在！");
            }
        }
    }
}
