// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MASA.PM.Web.Docs;

public static class ServiceCollectionExtensions
{
    public static void AddDocs(this IServiceCollection services, IConfiguration configuration)
    {
        var hostUrl = configuration.GetSection("gitlabconfig:HostUrl").Value;
        var apiToken = configuration.GetSection("gitlabconfig:ApiToken").Value;
        var fullPath = configuration.GetSection("gitlabconfig:FullPath").Value;

        GitLabClientWrapper? wrapper;
        if (hostUrl is null || apiToken is null || fullPath is null)
        {
            wrapper = new GitLabClientWrapper();
        }
        else
        {
            wrapper = new GitLabClientWrapper(hostUrl, apiToken, fullPath);
        }

        services.AddSingleton<GitLabClientWrapper>(_ => wrapper);
    }
}
