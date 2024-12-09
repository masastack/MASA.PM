// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Shared.Entities;

[Table("Clusters")]
public class Cluster : FullAggregateRoot<int, Guid>
{
    [Comment("Name")]
    [Required(ErrorMessage = "Cluster name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Cluster name length range is [2-100]")]
    public string Name { get; set; } = "";

    [Comment("Name")]
    [Required(ErrorMessage = "Cluster description is required")]
    [StringLength(255, ErrorMessage = "Cluster description length must be less than 255")]
    public string Description { get; set; } = "";

    public Cluster()
    {
    }

    public void SetCreatorAndModifier(Guid userId)
    {
        Creator = userId;
        Modifier = userId;
    }

    public Cluster(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
