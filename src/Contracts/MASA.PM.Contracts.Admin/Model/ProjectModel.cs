// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Contracts.Admin.Model
{
    public class ProjectModel
    {
        public int Id { get; set; }

        public string Identity { get; set; }

        public string Name { get; set; }

        public string LabelCode { get; set; }

        public Guid TeamId
        {
            get
            {
                return TeamIds != null && TeamIds.Count > 0 ? TeamIds[0] : Guid.Empty;
            }
        }

        public List<Guid>? TeamIds { get; set; }

        public List<AppModel> Apps { get; set; } = [];

        public ProjectModel(int id, string identity, string name, string labelCode, List<Guid>? teamIds)
        {
            Id = id;
            Identity = identity;
            Name = name;
            LabelCode = labelCode;
            TeamIds = teamIds;
        }
    }
}
