﻿using MASA.PM.Contracts.Base.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Contracts.Base.Model
{
    public class AddAppModel
    {
        private string _name = default!;

        [Required(ErrorMessage = "App name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "App name length range is [2-100]")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [Required(ErrorMessage = "EnvironmentClusterProjectId is required")]
        public List<int> EnvironmentClusterProjectIds { get; set; } = new();

        [EnumDataType(typeof(AppTypes), ErrorMessage = "App type error")]
        public AppTypes Type { get; set; }

        [EnumDataType(typeof(ServiceTypes), ErrorMessage = "App service type error")]
        public ServiceTypes ServiceType { get; set; }

        [Required(ErrorMessage = "Identity is required ")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Identity length range is [2-100]")]
        public string Identity { get; set; } = "";

        public string Url { get; set; } = "";

        public string SwaggerUrl { get; set; } = "";

        [StringLength(250, ErrorMessage = "App description length must be less than 250")]
        public string Description { get; set; } = "";

        public Guid ActionUserId { get; set; }
    }
}
