// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Shared.Entities;

[Table("EnvironmentClusterProjectApps")]
[Index(nameof(EnvironmentClusterProjectId), Name = "IX_EnvironmentClusterId")]
public class EnvironmentClusterProjectApp : Entity<int>
{
    [Comment("Environment cluster project Id")]
    [Range(1, int.MaxValue, ErrorMessage = "Environment cluster project is required")]
    public int EnvironmentClusterProjectId { get; set; }

    [Comment("App Id")]
    [Range(1, int.MaxValue, ErrorMessage = "App is required")]
    public int AppId { get; set; }

    [Comment("App URL")]
    [StringLength(255, MinimumLength = 0, ErrorMessage = "URL length range is [0-255]")]
    public string AppURL { get; set; } = "";

    [Comment("Swagger URL")]   
    [StringLength(255, MinimumLength = 0, ErrorMessage = "Swagger URL length range is [0-255]")]
    public string AppSwaggerURL { get; set; } = "";
}
