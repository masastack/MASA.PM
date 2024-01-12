// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using NGitLab;
using NGitLab.Models;

namespace MASA.PM.Web.Docs;

public class PMGitLabClient : GitLabClient
{
    private readonly Lazy<Task<Project>> _project;
    private IRepositoryClient? _repositoryClient;

    public async Task<Project> GetCurrentProjectAsync()
    {
        var project = await _project.Value;
        if (project is null)
        {
            throw new NullReferenceException(nameof(project));
        }

        return project;
    }

    public async Task<IRepositoryClient> GetCurrentRepositoryClientAsync()
    {
        if (_repositoryClient is null)
        {
            var project = await GetCurrentProjectAsync();
            _repositoryClient = this.GetRepository(project.Id);
        }

        return _repositoryClient;
    }

    public PMGitLabClient(string hostUrl, string apiToken, string pathWithNamespace) : base(hostUrl, apiToken)
    {
        _project = new Lazy<Task<Project>>(() => this.Projects.GetByNamespacedPathAsync(pathWithNamespace));
    }

    public PMGitLabClient(string hostUrl, string userName, string password, string pathWithNamespace) : base(hostUrl, userName, password)
    {
        _project = new Lazy<Task<Project>>(() => this.Projects.GetByNamespacedPathAsync(pathWithNamespace));
    }

    public PMGitLabClient(string hostUrl, string apiToken, RequestOptions options, string pathWithNamespace) : base(hostUrl, apiToken, options)
    {
        _project = new Lazy<Task<Project>>(() => this.Projects.GetByNamespacedPathAsync(pathWithNamespace));
    }

    public PMGitLabClient(string hostUrl, string userName, string password, RequestOptions options, string pathWithNamespace) : base(hostUrl,
        userName, password, options)
    {
        _project = new Lazy<Task<Project>>(() => this.Projects.GetByNamespacedPathAsync(pathWithNamespace));
    }
}
