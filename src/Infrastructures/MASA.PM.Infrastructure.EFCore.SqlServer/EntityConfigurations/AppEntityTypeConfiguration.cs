// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.SqlServer.EntityConfigurations;

internal class AppConfigEntityTypeConfiguration : IEntityTypeConfiguration<App>
{
    public void Configure(EntityTypeBuilder<App> builder)
    {
        builder.Property(m => m.Name).HasColumnType("nvarchar(100)");
        builder.Property(m => m.Identity).HasColumnType("varchar(100)");
        builder.Property(m => m.Type).HasColumnType("tinyint");
        builder.Property(m => m.ServiceType).HasColumnType("tinyint");
        builder.Property(m => m.Description).HasColumnType("nvarchar(255)");
        builder.Property(m => m.ModificationTime).HasDefaultValueSql("SYSDATETIME()");
    }
}
