// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Shared.Entities;

[Table("Apps")]
public class App : FullAggregateRoot<int, Guid>
{
    [Comment("Name")]
    [Required(ErrorMessage = "App name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "App name length range is [2-100]")]
    public string Name { get; set; } = "";

    /// <summary>
    /// App Id
    /// </summary>
    [Comment("Identity")]
    [Required(ErrorMessage = "App identity is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "App identity length range is [2-100]")]
    public string Identity { get; set; } = "";

    [Comment("Type")]
    [Range(1, int.MaxValue, ErrorMessage = "App type is required")]
    public AppTypes Type { get; set; }

    [Comment("ServiceType")]
    [Range(1, int.MaxValue, ErrorMessage = "App service type is required")]
    public ServiceTypes ServiceType { get; set; }

    [Comment("Description")]
    [StringLength(255, MinimumLength = 0, ErrorMessage = "Description length range is [0-255]")]
    public string Description { get; set; } = "";

    public List<AppResponsibilityUser>? ResponsibilityUsers { get; set; }

    public void SetCreatorAndModifier(Guid userId)
    {
        Creator = userId;
        Modifier = userId;
    }
}
