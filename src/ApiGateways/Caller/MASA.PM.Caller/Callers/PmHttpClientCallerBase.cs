﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.


namespace MASA.PM.Caller.Callers
{
    public class PmHttpClientCallerBase : StackHttpClientCaller
    {
        public PmHttpClientCallerBase(PMApiGatewayOptions options)
        {
            BaseAddress = options.PMServiceAddress;
        }

        protected override string BaseAddress { get; set; }
    }
}
