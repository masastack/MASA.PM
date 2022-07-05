// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Entities
{
    [Table("Labels")]
    [Index(nameof(Name), nameof(IsDeleted), Name = "IX_Name")]
    [Index(nameof(TypeCode), nameof(IsDeleted), Name = "IX_TypeCode")]
    public class Label : BaseEntity<int, Guid>
    {
        [Comment("Name")]
        [Required(ErrorMessage = "Label name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Label name length range is [2-100]")]
        public string Name { get; set; } = "";

        [Comment("TypeCode")]
        [Required(ErrorMessage = "TypeCode is required")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "TypeCode length range is [0-255]")]
        public string TypeCode { get; set; } = "";

        [Comment("TypeName")]
        [Required(ErrorMessage = "TypeName is required")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "TypeName length range is [0-255]")]
        public string TypeName { get; set; } = "";

        [Comment("Description")]
        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Description length range is [0-255]")]
        public string Description { get; set; } = "";
    }
}
