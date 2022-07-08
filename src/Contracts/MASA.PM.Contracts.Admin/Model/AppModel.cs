// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Model
{
    public class AppModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }

        public int ProjectId { get; set; }

        public AppTypes AppType { get; set; }

        public AppModel(int id, string name, string identity, int projectId, AppTypes appType)
        {
            Id = id;
            Name = name;
            Identity = identity;
            ProjectId = projectId;
            AppType = appType;
        }
    }
}
