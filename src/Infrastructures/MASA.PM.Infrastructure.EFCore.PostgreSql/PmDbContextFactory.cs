// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.PostgreSql;

internal class PmDbContextFactory : IDesignTimeDbContextFactory<PmDbContext>
{
    const string ConnectionStringKey = "MasaPmPgsqlStaging";

    public PmDbContext CreateDbContext(string[] args)
    {
        PmDbContext.RegistAssembly(typeof(PmDbContextFactory).Assembly);
        var configuration = new ConfigurationBuilder()
             .AddUserSecrets(typeof(PmDbContextFactory).Assembly, optional: true)
             .Build();

        var connectionString = configuration[ConnectionStringKey]!;
        var optionsBuilder = new MasaDbContextOptionsBuilder<PmDbContext>();
        optionsBuilder.DbContextOptionsBuilder.UseNpgsql(connectionString, m => m.MigrationsAssembly("MASA.PM.Infrastructure.EFCore.PostgreSql"));
        return new PmDbContext(optionsBuilder.MasaOptions);
    }
}
