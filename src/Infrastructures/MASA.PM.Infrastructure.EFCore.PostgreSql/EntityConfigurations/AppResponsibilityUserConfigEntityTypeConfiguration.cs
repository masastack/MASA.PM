// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.PostgreSql.EntityConfigurations;

internal class AppResponsibilityUserConfigEntityTypeConfiguration : IEntityTypeConfiguration<AppResponsibilityUser>
{
    public void Configure(EntityTypeBuilder<AppResponsibilityUser> builder)
    {
        builder.Property(m => m.CreateTime).HasDefaultValueSql("now()");
        builder.HasOne(m => m.App).WithMany(m => m.ResponsibilityUsers);
    }
}
