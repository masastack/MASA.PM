using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Model
{
    public class AppModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }

        public int ProjectId { get; set; }

        public List<int> EnvironmentIds { get; set; } = new();

        public AppModel(int id, string name, string identity, int projectId, IEnumerable<int> envIds)
        {
            Id = id;
            Name = name;
            Identity = identity;
            ProjectId = projectId;
            EnvironmentIds = envIds.ToList();
        }
    }
}
