// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Shared.Entities;

[Table("EnvironmentProjectTeam")]
public class EnvironmentProjectTeam
{
    [Required]
    public int ProjectId { get; set; }

    [Required]
    public string EnvironmentName { get; set; } = "";

    [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "TeamId is required")]
    public Guid TeamId { get; set; }
}
