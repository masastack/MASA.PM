﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class UpdateClusterDto : AddClusterWhitEnvironmentsDto
    {
        public int ClusterId { get; set; }
    }
}
