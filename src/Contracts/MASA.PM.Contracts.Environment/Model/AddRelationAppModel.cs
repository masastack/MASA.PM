using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class AddRelationAppModel
    {
        public int AppId { get; set; }

        public int EnvironmentClusterId { get; set; }

        public int ProjectId { get; set; }
    }
}
