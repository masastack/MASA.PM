// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class PmHttpClientCallerBase : HttpClientCallerBase
    {
        private readonly TokenProvider _tokenProvider;

        public PmHttpClientCallerBase(
            IServiceProvider serviceProvider,
            TokenProvider tokenProvider,
            PMApiGatewayOptions options) : base(serviceProvider)
        {
            BaseAddress = options.PMServiceAddress;
            _tokenProvider = tokenProvider;
        }

        protected override string BaseAddress { get; set; }

        protected override async Task ConfigHttpRequestMessageAsync(HttpRequestMessage requestMessage)
        {
            if (!string.IsNullOrWhiteSpace(_tokenProvider.AccessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.AccessToken);
            }

            await base.ConfigHttpRequestMessageAsync(requestMessage);
        }
    }
}
