// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.PostgreSql.EntityConfigurations;

internal class AppConfigEntityTypeConfiguration : IEntityTypeConfiguration<App>
{
    public void Configure(EntityTypeBuilder<App> builder)
    {
        builder.Property(m => m.Name).HasColumnType("varchar(100)");
        builder.Property(m => m.Identity).HasColumnType("varchar(100)");
        builder.Property(m => m.Type).HasColumnType("smallint");
        builder.Property(m => m.ServiceType).HasColumnType("smallint");
        builder.Property(m => m.Description).HasColumnType("varchar(255)");
        builder.Property(m => m.ModificationTime).HasDefaultValueSql("now()");
    }
}
