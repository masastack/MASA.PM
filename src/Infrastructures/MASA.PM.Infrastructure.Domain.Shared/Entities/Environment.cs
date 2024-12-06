// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Shared.Entities;

[Table("Environments")]
public class Environment : FullAggregateRoot<int, Guid>
{
    [Comment("Name")]
    [Required(ErrorMessage = "Environment name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Environment name length range is [2-100]")]
    public string Name { get; set; } = "";

    [Comment("Description")]
    [Required(ErrorMessage = "Environment description is required")]
    [StringLength(255, ErrorMessage = "Environment description length must be less than 255")]
    public string Description { get; set; } = "";

    [Comment("Color")]
    [Required(ErrorMessage = "Environment color is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Environment color length range is [2-10]")]
    public string Color { get; set; } = "";

    public Environment()
    {
    }

    public void SetCreatorAndModifier(Guid userId)
    {
        Creator = userId;
        Modifier = userId;
    }

    public Environment(int id, string name, string description, string color)
    {
        Id = id;
        Name = name;
        Description = description;
        Color = color;
    }
}
