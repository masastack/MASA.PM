// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.SqlServer.EntityConfigurations;

internal class EnvironmentProjectTeamEntityTypeConfiguration : IEntityTypeConfiguration<EnvironmentProjectTeam>
{
    public void Configure(EntityTypeBuilder<EnvironmentProjectTeam> builder)
    {
        builder.HasKey(t => new { t.ProjectId, t.TeamId, t.EnvironmentName });
    }
}
