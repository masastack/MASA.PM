// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project);

        Task AddEnvironmentClusterProjectsAsync(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task<List<Project>> GetListByTeamIdsAsync(List<Guid> teamIds);

        Task<Project> GetAsync(int Id);

        Task<List<Project>> GetListAsync();

        Task<List<Project>> GetListByEnvironmentClusterIdAsync(int environmentClusterId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAsync(int projectId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(int projectId, IEnumerable<int> environmentClusterIds);

        Task<List<int>> GetEnvironmentClusterProjectIdsByEnvClusterIdsAndProjectId(IEnumerable<int> envClusterIds, int projectId);

        Task<List<Label>> GetProjectTypesAsync();

        Task<List<Project>> GetProjectListByEnvIdAsync(string envName);

        Task UpdateAsync(Project cluster);

        Task RemoveAsync(int Id);

        Task RemoveEnvironmentClusterProjects(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task IsExistedProjectName(string name, List<int> environmentClusterIds, params int[] excludeProjectIds);
    }
}
