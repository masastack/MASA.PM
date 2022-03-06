namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project);

        Task AddEnvironmentClusterProjectsAsync(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task<Project> GetAsync(int Id);

        Task<List<ProjectsViewModel>> GetListByEnvironmentClusterIdAsync(int environmentClusterId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAsync(int projectId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(int projectId, IEnumerable<int> environmentIds);

        Task UpdateAsync(Project cluster);

        Task DeleteAsync(int Id);

        Task DeleteEnvironmentClusterProjects(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);
    }
}
