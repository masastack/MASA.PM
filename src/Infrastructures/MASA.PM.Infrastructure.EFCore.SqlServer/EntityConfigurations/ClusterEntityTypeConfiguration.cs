// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.SqlServer.EntityConfigurations;

internal class ClusterEntityTypeConfiguration : IEntityTypeConfiguration<Cluster>
{
    public void Configure(EntityTypeBuilder<Cluster> builder)
    {
        builder.Property(m => m.Name).HasColumnType("nvarchar(100)");
        builder.Property(m => m.Description).HasColumnType("nvarchar(255)");
        builder.Property(m => m.ModificationTime).HasDefaultValueSql("SYSDATETIME()");
    }
}
