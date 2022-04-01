using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Model
{
    public class ProjectModel
    {
        public int Id { get; set; }

        public string Identity { get; set; }

        public string Name { get; set; }

        public int LabelId { get; set; }

        public Guid TeamId { get; set; }

        public List<int> EnvironmentIds { get; set; } = new();

        public ProjectModel(int id, string identity, string name, int labelId, Guid teamId, IEnumerable<int> envIds)
        {
            Id = id;
            Identity = identity;
            Name = name;
            LabelId = labelId;
            TeamId = teamId;
            EnvironmentIds = envIds.ToList();
        }
    }
}
