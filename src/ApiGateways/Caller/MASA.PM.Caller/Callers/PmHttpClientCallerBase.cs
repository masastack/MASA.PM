// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller.Callers
{
    public class PmHttpClientCallerBase : HttpClientCallerBase
    {
        public PmHttpClientCallerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override string BaseAddress { get; set; } = AppSettings.Get("PmServiceBaseAddress");
    }
}
