﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Model
{
    public class AppModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }

        public int ProjectId { get; set; }

        public AppTypes Type { get; set; }

        public ServiceTypes ServiceType { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string SwaggerUrl { get; set; }

        public AppModel(int id, string name, string identity, int projectId, AppTypes type, string url, ServiceTypes serviceType, string swaggerUrl, string description)
        {
            Id = id;
            Name = name;
            Identity = identity;
            ProjectId = projectId;
            Type = type;
            ServiceType = serviceType;
            Description = description;
            Url = url;
            SwaggerUrl = swaggerUrl;
        }
    }
}
