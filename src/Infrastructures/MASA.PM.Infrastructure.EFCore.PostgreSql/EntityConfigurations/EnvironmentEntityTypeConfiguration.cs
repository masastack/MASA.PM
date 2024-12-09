// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.PostgreSql.EntityConfigurations;

internal class EnvironmentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Shared.Entities.Environment>
{
    public void Configure(EntityTypeBuilder<Domain.Shared.Entities.Environment> builder)
    {
        builder.Property(t => t.ModificationTime).HasDefaultValueSql("now()");
    }
}
