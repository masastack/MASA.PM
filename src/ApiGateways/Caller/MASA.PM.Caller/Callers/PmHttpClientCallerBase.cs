// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace MASA.PM.Caller.Callers
{
    public class PmHttpClientCallerBase : HttpClientCallerBase
    {
        public PmHttpClientCallerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override string BaseAddress { get; set; } = AppSettings.Get("ServiceBaseUrl");

        protected override IHttpClientBuilder UseHttpClient()
        {
            return base.CallerOptions.UseHttpClient(delegate (MasaHttpClientBuilder opt)
            {
                opt.Name = Name;
                opt.Configure = (HttpClient client) =>
                {
                    client.BaseAddress = new Uri(BaseAddress);
                };
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        }
    }
}
