﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Project;

public class ProjectCommandHandler
{
    private readonly IProjectRepository _projectRepository;

    public ProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [EventHandler]
    public async Task AddProjectAsync(AddProjectCommand command)
    {
        var project = new Shared.Entities.Project
        {
            Identity = command.ProjectModel.Identity,
            LabelCode = command.ProjectModel.LabelCode,
            Name = command.ProjectModel.Name,
            Description = command.ProjectModel.Description,
        };
        var newProject = await _projectRepository.AddAsync(project);

        var environmentClusterProjects = command.ProjectModel.EnvironmentClusterIds.Select(environmentClusterId => new EnvironmentClusterProject
        {
            EnvironmentClusterId = environmentClusterId,
            ProjectId = newProject.Id
        });

        await _projectRepository.AddEnvironmentClusterProjectsAsync(environmentClusterProjects);
        await AddProjectTeams(newProject.Id, command.ProjectModel);
    }

    [EventHandler]
    public async Task UpdateProjectAsync(UpdateProjectCommand command)
    {
        var project = await _projectRepository.GetAsync(command.ProjectModel.ProjectId);

        project.LabelCode = command.ProjectModel.LabelCode;
        project.Name = command.ProjectModel.Name;
        project.Description = command.ProjectModel.Description;

        await _projectRepository.UpdateAsync(project);

        var oldEnvironmentClusterIds = (
                await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectModel.ProjectId)
            )
            .Select(environmentClusterProject => environmentClusterProject.EnvironmentClusterId)
            .ToList();

        //need to delete EnvironmentClusterProject
        var deleteEnvironmentClusterIds = oldEnvironmentClusterIds.Except(command.ProjectModel.EnvironmentClusterIds);
        if (deleteEnvironmentClusterIds.Any())
        {
            var deleteEnvironmentClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsById(deleteEnvironmentClusterIds, command.ProjectModel.ProjectId);
            await _projectRepository.RemoveEnvironmentClusterProjects(deleteEnvironmentClusterProjects);
        }

        //need to add EnvironmentClusterProject
        var addEnvironmentClusterIds = command.ProjectModel.EnvironmentClusterIds.Except(oldEnvironmentClusterIds).ToList();
        if (addEnvironmentClusterIds.Any())
        {
            await _projectRepository.IsExistedProjectName(command.ProjectModel.Name, addEnvironmentClusterIds);
            await _projectRepository.AddEnvironmentClusterProjectsAsync(addEnvironmentClusterIds.Select(environmentClusterId => new EnvironmentClusterProject
            {
                EnvironmentClusterId = environmentClusterId,
                ProjectId = command.ProjectModel.ProjectId
            }));
        }

        await _projectRepository.RemoveEnvironemtProjectTeamAsync(command.ProjectModel.ProjectId, command.ProjectModel.EnvironmentName);
        await AddProjectTeams(project.Id, command.ProjectModel);
    }

    [EventHandler]
    public async Task RemoveProjectAsync(RemoveProjectCommand command)
    {
        await _projectRepository.RemoveAsync(command.ProjectId);

        var environmentClusterProjects = await _projectRepository.GetEnvironmentClusterProjectsByProjectIdAsync(command.ProjectId);
        await _projectRepository.RemoveEnvironmentClusterProjects(environmentClusterProjects);

        await _projectRepository.RemoveProjectEnvironemtTeamsAsync(command.ProjectId);
    }

    private async Task AddProjectTeams(int projectId,AddProjectDto projectModel)
    {
        await _projectRepository.AddEnvironemtProjectTeamsAsync(
            projectModel.TeamIds.Select(teamId => new EnvironmentProjectTeam
            {
                EnvironmentName = projectModel.EnvironmentName,
                TeamId = teamId,
                ProjectId = projectId
            }));
    }
}
