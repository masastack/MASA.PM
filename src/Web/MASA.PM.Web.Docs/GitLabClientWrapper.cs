// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using NGitLab;
using NGitLab.Models;

namespace MASA.PM.Web.Docs;

public class GitLabClientWrapper
{
    private readonly Lazy<Task<Project>> _project;
    private IRepositoryClient? _repositoryClient;

    public GitLabClient? GitLabClient { get; }

    public GitLabClientWrapper()
    {
    }

    public GitLabClientWrapper(string hostUrl, string apiToken, string pathWithNamespace)
    {
        GitLabClient = new GitLabClient(hostUrl, apiToken);

        _project = new Lazy<Task<Project>>(async () =>
        {
            if (GitLabClient is null)
            {
                throw new InvalidOperationException("The configuration for GitLab is missing.");
            }

            return await GitLabClient.Projects.GetByNamespacedPathAsync(pathWithNamespace);
        });
    }

    public async Task<Project> GetCurrentProjectAsync()
    {
        if (_project is null)
        {
            throw new InvalidOperationException("The configuration for GitLab is missing.");
        }

        var project = await _project.Value;

        return project;
    }

    public async Task<IRepositoryClient> GetCurrentRepositoryClientAsync()
    {
        if (_repositoryClient is not null)
        {
            return _repositoryClient;
        }

        var project = await GetCurrentProjectAsync();
        _repositoryClient = GitLabClient!.GetRepository(project.Id);

        return _repositoryClient;
    }
}
