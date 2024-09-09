// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IProjectRepository
    {
        Task<Project> AddAsync(Project project);

        Task AddEnvironmentClusterProjectsAsync(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task<EnvironmentClusterProject> AddEnvironmentClusterProjectAsync(EnvironmentClusterProject environmentClusterProject);

        Task<(List<Project>, List<EnvironmentProjectTeam>)> GetListByTeamIdsAsync(List<Guid> teamIds, string environment);

        Task<Project> GetAsync(int Id);

        Task<Project> GetByIdentityAsync(string identity);

        Task<List<Project>> GetListAsync();

        Task<List<Project>> GetListByEnvironmentClusterIdAsync(int environmentClusterId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAsync(int projectId);

        Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsById(IEnumerable<int> envClusterIds, int projectId);

        Task<List<Project>> GetProjectListByEnvIdAsync(string envName);

        Task UpdateAsync(Project cluster);

        Task RemoveAsync(int Id);

        Task RemoveEnvironmentClusterProjects(IEnumerable<EnvironmentClusterProject> environmentClusterProjects);

        Task IsExistedProjectName(string name, List<int> environmentClusterIds, params int[] excludeProjectIds);

        Task<bool> IsExistProjectInCluster(int clusterId);

        Task AddEnvironemtProjectTeamAsync(EnvironmentProjectTeam environmentProjectTeam);

        Task AddEnvironemtProjectTeamsAsync(IEnumerable<EnvironmentProjectTeam> environmentProjectTeams);

        Task RemoveEnvironemtProjectTeamAsync(int projectId, string environemntName);

        Task RemoveProjectEnvironemtTeamsAsync(int projectId);

        Task<List<EnvironmentProjectTeam>> GetProjectTeamByProjectId(int projectId);

        Task<List<EnvironmentProjectTeam>> GetProjectTeamByProjectIds(IEnumerable<int> projectIds);
    }
}
