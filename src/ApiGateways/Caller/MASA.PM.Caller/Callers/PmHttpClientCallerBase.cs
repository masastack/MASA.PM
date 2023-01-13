// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class PmHttpClientCallerBase : HttpClientCallerBase
    {
        public PmHttpClientCallerBase(
            IServiceProvider serviceProvider,
            PMApiGatewayOptions options) : base(serviceProvider)
        {
            BaseAddress = options.PMServiceAddress;
        }

        protected override string BaseAddress { get; set; }

        protected override IHttpClientBuilder UseHttpClient()
        {
            return base.UseHttpClient().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        }
    }
}
