using MASA.PM.Contracts.Base.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class UpdateAppModel : AddAppModel
    {
        [Required]
        public int Id { get; set; }
    }
}
