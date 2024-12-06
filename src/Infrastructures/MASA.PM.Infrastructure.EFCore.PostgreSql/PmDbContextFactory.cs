// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.PostgreSql;

internal class PmDbContextFactory : IDesignTimeDbContextFactory<PmDbContext>
{
    public PmDbContext CreateDbContext(string[] args)
    {
        PmDbContext.RegistAssembly(typeof(PmDbContextFactory).Assembly);
        var optionsBuilder = new MasaDbContextOptionsBuilder<PmDbContext>();
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder
            .AddJsonFile("migration-pgsql.json")
            .Build();
        optionsBuilder.DbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), m => m.MigrationsAssembly("MASA.PM.Infrastructure.EFCore.PostgreSql"));
        return new PmDbContext(optionsBuilder.MasaOptions);
    }
}
