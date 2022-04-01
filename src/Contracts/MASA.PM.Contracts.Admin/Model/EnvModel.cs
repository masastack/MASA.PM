using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Model
{
    public class EnvModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EnvModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
