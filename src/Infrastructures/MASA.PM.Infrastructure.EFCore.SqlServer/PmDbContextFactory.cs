// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore.SqlServer;

internal class PmDbContextFactory : IDesignTimeDbContextFactory<PmDbContext>
{
    public PmDbContext CreateDbContext(string[] args)
    {
        PmDbContext.RegistAssembly(typeof(PmDbContextFactory).Assembly);
        var optionsBuilder = new MasaDbContextOptionsBuilder<PmDbContext>();
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder
            .AddJsonFile("migration-sqlserver.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),m=>m.MigrationsAssembly("MASA.PM.Infrastructure.EFCore.SqlServer"));
        return new PmDbContext(optionsBuilder.MasaOptions);
    }
}
