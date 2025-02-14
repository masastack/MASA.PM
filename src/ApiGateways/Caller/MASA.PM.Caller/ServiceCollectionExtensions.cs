// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPMApiGateways(this IServiceCollection services, Action<PMApiGatewayOptions>? configure = null)
    {
        var options = new PMApiGatewayOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);
        services.AddStackCaller(Assembly.Load("MASA.PM.Caller"));
        //services.AddStackCaller(Assembly.Load("MASA.PM.Caller"), jwtTokenValidatorOptions =>
        //{
        //    jwtTokenValidatorOptions.AuthorityEndpoint = options.AuthorityEndpoint;
        //}, clientRefreshTokenOptions =>
        //{
        //    clientRefreshTokenOptions.ClientId = options.ClientId;
        //    clientRefreshTokenOptions.ClientSecret = options.ClientSecret;
        //});

        return services;
    }
}
