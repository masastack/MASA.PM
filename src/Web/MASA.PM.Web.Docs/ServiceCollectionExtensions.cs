// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MASA.PM.Web.Docs;

public static class ServiceCollectionExtensions
{
    public static void AddDocs(this IServiceCollection services, IConfiguration configuration)
    {
        var gitLabClient = new PMGitLabClient(configuration["gitlabconfig:HostUrl"], configuration["gitlabconfig:ApiToken"],
            configuration["gitlabconfig:FullPath"]);

        services.AddSingleton<PMGitLabClient>(_ => gitLabClient);
    }
}
