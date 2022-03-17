using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Admin.Dto
{
    public class InitDto
    {
        [Required]
        public string ClusterName { get; set; } = "";

        [Required]
        public string ClusterDescription { get; set; } = "";

        public List<AddEnvironmentDto> Environments { get; set; } = new();
    }
}
