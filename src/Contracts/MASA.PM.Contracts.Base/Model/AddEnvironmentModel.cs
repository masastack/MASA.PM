﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Environment.Model
{
    public class AddEnvironmentModel
    {
        private string _name = default!;

        [Display(Name = "Environment name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} length range is [2-100]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [StringLength(250, MinimumLength = 0, ErrorMessage = "Environment description length must be less than 250")]
        public string Description { get; set; } = "";
    }
}
