// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Domain.Shared.Entities;

[Table("AppResponsibilityUsers")]
public class AppResponsibilityUser : Entity<int>
{
    public int AppId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreateTime { get; set; }

    public App App { get; set; } = default!;
}
