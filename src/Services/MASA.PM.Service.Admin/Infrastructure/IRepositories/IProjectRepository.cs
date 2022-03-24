namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project);

        Task AddEnvironmentClusterProjectsAsync(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task<List<Project>> GetListByTeamIdAsync(Guid teamId);

        Task<Project> GetAsync(int Id);

        Task<List<Project>> GetListByEnvironmentClusterIdAsync(int environmentClusterId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAsync(int projectId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(int projectId, IEnumerable<int> environmentClusterIds);

        Task<List<int>> GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(IEnumerable<int> envClusterIds, int projectId);

        Task<List<Label>> GetProjectTypesAsync();

        Task UpdateAsync(Project cluster);

        Task RemoveAsync(int Id);

        Task RemoveEnvironmentClusterProjects(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task IsExistedProjectName(string name, List<int> environmentClusterIds, params int[] excludeProjectIds);
    }
}
