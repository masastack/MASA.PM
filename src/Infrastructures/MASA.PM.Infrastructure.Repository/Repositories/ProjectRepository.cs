// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Repository.Repositories;

internal class ProjectRepository : IProjectRepository
{
    private readonly PmDbContext _dbContext;
    private readonly II18n<DefaultResource> _i18N;
    public ProjectRepository(PmDbContext dbContext, II18n<DefaultResource> i18N)
    {
        _dbContext = dbContext;
        _i18N = i18N;
    }

    public async Task<Project> AddAsync(Project project)
    {
        if (await _dbContext.Projects.AnyAsync(p => p.Name == project.Name))
        {
            throw new UserFriendlyException(_i18N.T("Project name already exists!"));
        }
        if (await _dbContext.Projects.AnyAsync(p => p.Identity == project.Identity))
        {
            throw new UserFriendlyException(_i18N.T("Project ID already exists!"));
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

    public async Task<EnvironmentClusterProject> AddEnvironmentClusterProjectAsync(EnvironmentClusterProject environmentClusterProject)
    {
        await _dbContext.AddAsync(environmentClusterProject);
        await _dbContext.SaveChangesAsync();

        return environmentClusterProject;
    }

    public async Task<(List<Project>, List<EnvironmentProjectTeam>)> GetListByTeamIdsAsync(List<Guid> teamIds, string environment)
    {
        var projectTeams = (await _dbContext.EnvironmentProjectTeams
            .Where(c => environment.Equals(c.EnvironmentName))
            .ToListAsync())
            .DistinctBy(c => new { c.ProjectId, c.TeamId })
            .ToList();

        var projectIds = projectTeams.Where(p => teamIds.Contains(p.TeamId)).GroupBy(p => p.ProjectId).Select(p => p.Key).ToList();

        var projects = await _dbContext.Projects.Where(project => projectIds.Contains(project.Id)).ToListAsync();

        return new ValueTuple<List<Project>, List<EnvironmentProjectTeam>>(projects, projectTeams);
    }

    public async Task RemoveAsync(int Id)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(project => project.Id == Id);
        if (project == null)
        {
            throw new UserFriendlyException(_i18N.T("Project does not exist!"));
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveEnvironmentClusterProjects(IEnumerable<EnvironmentClusterProject> environmentClusterProjects)
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

        return result ?? throw new UserFriendlyException(_i18N.T("Project does not exist!"));
    }

    public async Task<Project> GetByIdentityAsync(string identity)
    {
        var result = await _dbContext.Projects.FirstOrDefaultAsync(project => project.Identity == identity);

        return result ?? throw new UserFriendlyException(_i18N.T("Project does not exist!"));
    }

    public async Task<List<Project>> GetListAsync()
    {
        var result = await _dbContext.Projects.ToListAsync();

        return result;
    }

    public async Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAsync(int projectId)
    {
        var result = await _dbContext.EnvironmentClusterProjects.Where(environmentClusterProject => environmentClusterProject.ProjectId == projectId).ToListAsync();

        return result;
    }

    public async Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsByProjectIdAndEnvirionmentClusterIds(int projectId, IEnumerable<int> environmentClusterIds)
    {
        var result = await _dbContext.EnvironmentClusterProjects
            .Where(ecp => ecp.ProjectId == projectId && environmentClusterIds.Contains(ecp.EnvironmentClusterId))
            .ToListAsync();

        return result;
    }

    public async Task<List<EnvironmentClusterProject>> GetEnvironmentClusterProjectsById(IEnumerable<int> envClusterIds, int projectId)
    {
        var result = await _dbContext.EnvironmentClusterProjects
            .Where(ecp => envClusterIds.Contains(ecp.EnvironmentClusterId) && ecp.ProjectId == projectId)
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

    public async Task<List<Project>> GetProjectListByEnvIdAsync(string envName)
    {
        var projects = await (from env in _dbContext.Environments.Where(env => env.Name == envName)
                              join envCluster in _dbContext.EnvironmentClusters on env.Id equals envCluster.EnvironmentId
                              join envClusterProject in _dbContext.EnvironmentClusterProjects on envCluster.Id equals envClusterProject.EnvironmentClusterId
                              join project in _dbContext.Projects on envClusterProject.ProjectId equals project.Id
                              select project)
                          .Distinct()
                          .ToListAsync();

        return projects;
    }

    public async Task UpdateAsync(Project project)
    {
        if (await _dbContext.Projects.AnyAsync(e => e.Name.ToLower() == project.Name.ToLower() && e.Id != project.Id))
        {
            throw new UserFriendlyException(_i18N.T("Project name already exists!"));
        }

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
            string message = _i18N.T("The project name [{name}] already exists in the environment [{EnvironmentName}]/environment [{ClusterName}]!")
                .Replace("{name}", name)
                .Replace("{EnvironmentName}", result.EnvironmentName)
                .Replace("{ClusterName}", result.ClusterName);
            throw new UserFriendlyException(message);
        }
    }

    public async Task<bool> IsExistProjectInCluster(int clusterId)
    {
        var result = await (from envCluster in _dbContext.EnvironmentClusters
                            join envClusterProject in _dbContext.EnvironmentClusterProjects on envCluster.Id equals envClusterProject.EnvironmentClusterId
                            select envCluster
                         ).AnyAsync(ec => ec.ClusterId == clusterId);

        return result;
    }


    public async Task AddEnvironemtProjectTeamAsync(EnvironmentProjectTeam environmentProjectTeam)
    {
        await _dbContext.EnvironmentProjectTeams.AddAsync(environmentProjectTeam);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddEnvironemtProjectTeamsAsync(IEnumerable<EnvironmentProjectTeam> environmentProjectTeams)
    {
        await _dbContext.EnvironmentProjectTeams.AddRangeAsync(environmentProjectTeams);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveEnvironemtProjectTeamAsync(int projectId, string environemntName)
    {
        var envProjectTeams = await _dbContext.EnvironmentProjectTeams.Where(c => c.ProjectId == projectId && c.EnvironmentName == environemntName).ToListAsync();
        if (envProjectTeams != null && envProjectTeams.Any())
        {
            _dbContext.EnvironmentProjectTeams.RemoveRange(envProjectTeams);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveProjectEnvironemtTeamsAsync(int projectId)
    {
        var envProjectTeams = await _dbContext.EnvironmentProjectTeams.Where(c => c.ProjectId == projectId).ToListAsync();
        if (envProjectTeams != null && envProjectTeams.Any())
        {
            _dbContext.EnvironmentProjectTeams.RemoveRange(envProjectTeams);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<EnvironmentProjectTeam>> GetProjectTeamByProjectId(int projectId)
    {
        var projectTeams = await _dbContext.EnvironmentProjectTeams
            .Where(c => c.ProjectId == projectId)
            .ToListAsync();

        return projectTeams;
    }

    public async Task<List<EnvironmentProjectTeam>> GetProjectTeamByProjectIds(IEnumerable<int> projectIds)
    {
        var projectTeams = await _dbContext.EnvironmentProjectTeams
            .Where(c => projectIds.Contains(c.ProjectId))
            .ToListAsync();

        return projectTeams;
    }

}
