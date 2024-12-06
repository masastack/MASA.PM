// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.PostgreSql.EntityConfigurations;

internal class EnvironmentClusterProjectAppEntityTypeConfiguration : IEntityTypeConfiguration<EnvironmentClusterProjectApp>
{
    public void Configure(EntityTypeBuilder<EnvironmentClusterProjectApp> builder)
    {
        builder.Property(m => m.AppURL).HasColumnType("varchar(255)");
        builder.Property(m => m.AppSwaggerURL).HasColumnType("varchar(255)");
    }
}
