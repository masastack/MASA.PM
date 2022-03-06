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
            if (_dbContext.Projects.Any(p => p.Name.ToLower() == project.Name.ToLower()))
            {
                throw new Exception("项目名称已存在！");
            }

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

        public async Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(int projectId, IEnumerable<int> environmentIds)
        {
            var result = await _dbContext.EnvironmentClusterProjects.Where(ecp => ecp.ProjectId == projectId && environmentIds.Contains(ecp.EnvironmentClusterId)).ToListAsync();

            return result;
        }

        public async Task<List<ProjectsViewModel>> GetListByEnvironmentClusterIdAsync(int environmentClusterId)
        {
            System.Linq.Expressions.Expression<Func<EnvironmentClusterProject, bool>> predicate = environmentClusterProject =>
                                    environmentClusterProject.EnvironmentClusterId == environmentClusterId;

            var result = await _dbContext.EnvironmentClusterProjects.Where(predicate)
                .Join(
                    _dbContext.Projects,
                    environmentClusterProject => environmentClusterProject.ProjectId,
                    project => project.Id,
                    (environmentClusterProject, project) => new { EnvironmentClusterProjectId = environmentClusterProject.Id, project }
                )
                .Select(projectGroup => new ProjectsViewModel
                {
                    Id = projectGroup.project.Id,
                    Name = projectGroup.project.Name,
                    Description = projectGroup.project.Description,
                    EnvironmentClusterProjectId = projectGroup.EnvironmentClusterProjectId,
                    Modifier = projectGroup.project.Modifier,
                    ModificationTime = projectGroup.project.ModificationTime,
                })
                .ToListAsync();

            return result;
        }

        public async Task UpdateAsync(Project project)
        {
            if (_dbContext.Projects.Any(e => e.Name.ToLower() == project.Name.ToLower() && e.Id != project.Id))
            {
                throw new Exception("项目名称已存在！");
            }

            _dbContext.Projects.Update(project);
            await _dbContext.SaveChangesAsync();
        }
    }
}
