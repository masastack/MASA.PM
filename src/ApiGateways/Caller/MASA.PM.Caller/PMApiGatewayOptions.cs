// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Caller
{
    public class PMApiGatewayOptions
    {
        public string PMServiceAddress { get; set; }

        public PMApiGatewayOptions(string url)
        {
            PMServiceAddress = url;
        }
    }
}
