// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Projects")]
    public class Project : FullAggregateRoot<int, Guid>
    {
        [Comment("Identity")]
        [Required(ErrorMessage = "Project identity is required")]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "App identity length range is [2-100]")]
        public string Identity { get; set; } = "";

        [Comment("LabelCode")]
        public string LabelCode { get; set; } = "";

        [Comment("Name")]
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "System name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("Description")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Description length range is [0-255]")]
        public string Description { get; set; } = "";

        public void SetCreatorAndModifier(Guid userId)
        {
            Creator = userId;
            Modifier = userId;
        }
    }
}
