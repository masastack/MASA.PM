﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPMApiGateways(this IServiceCollection services, Action<PMApiGatewayOptions>? configure = null)
    {
        var options = new PMApiGatewayOptions("http://localhost:18002/");

        configure?.Invoke(options);
        services.AddSingleton(options);
        services.AddCaller(Assembly.Load("MASA.PM.Caller"));

        return services;
    }
}