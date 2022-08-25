// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Dto
{
    public class EnvironmentClusterDto
    {
        public int Id { get; set; }

        public string EnvironmentName { get; set; } = "";

        public string EnvironmentColor { get; set; } = "";

        public string ClusterName { get; set; } = "";

        public string EnvironmentClusterName
        {
            get
            {
                return $"{EnvironmentName} {ClusterName}";
            }
        }

        public string AppURL { get; set; } = "";

        public string AppSwaggerURL { get; set; } = "";
    }
}
