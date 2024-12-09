// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection;

public static class PmPgsqlExtensition
{
    public static MasaDbContextBuilder UsePgsql(
        this MasaDbContextBuilder builder,
        Action<NpgsqlDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        var name = ConnectionStringNameAttribute.GetConnStringName(builder.DbContextType);
        builder.Builder = (serviceProvider, dbContextOptionsBuilder) =>
        {
            var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
            dbContextOptionsBuilder.UseNpgsql(
                connectionStringProvider.GetConnectionString(name),
                sqlServerOptionsAction);
        };
        return builder;
    }

    public static MasaDbContextBuilder UsePgsql(
        this MasaDbContextBuilder builder,
        string connectionString,
        Action<NpgsqlDbContextOptionsBuilder>? sqlServerOptionsAction = null)
        => builder.UsePgCore(connectionString, sqlServerOptionsAction);

    public static MasaDbContextBuilder UsePgsql(
        this MasaDbContextBuilder builder,
        DbConnection connection,
        Action<NpgsqlDbContextOptionsBuilder>? sqlServerOptionsAction = null)
        => builder.UsePgCore(connection, sqlServerOptionsAction);

    private static MasaDbContextBuilder UsePgCore(
        this MasaDbContextBuilder builder,
        string connectionString,
        Action<NpgsqlDbContextOptionsBuilder>? sqlServerOptionsAction)
    {
        builder.Builder = (_, dbContextOptionsBuilder)
            => dbContextOptionsBuilder.UseNpgsql(connectionString, sqlServerOptionsAction);
        return builder.ConfigMasaDbContextAndConnectionStringRelations(connectionString);
    }

    private static MasaDbContextBuilder UsePgCore(
        this MasaDbContextBuilder builder,
        DbConnection connection,
        Action<NpgsqlDbContextOptionsBuilder>? sqlServerOptionsAction = null)
    {
        builder.Builder = (_, dbContextOptionsBuilder) => dbContextOptionsBuilder.UseNpgsql(connection, sqlServerOptionsAction);
        return builder.ConfigMasaDbContextAndConnectionStringRelations(connection.ConnectionString);
    }

    internal static MasaDbContextBuilder ConfigMasaDbContextAndConnectionStringRelations(
        this MasaDbContextBuilder builder,
        string connectionString)
    {
        var name = ConnectionStringNameAttribute.GetConnStringName(builder.DbContextType);

        builder.Services.Configure<ConnectionStrings>(connectionStrings =>
        {
            if (connectionStrings.ContainsKey(name) &&
                connectionString != connectionStrings.GetConnectionString(name))
                throw new ArgumentException($"The [{builder.DbContextType.Name}] Database Connection String already exists");

            connectionStrings.TryAdd(name, connectionString);
        });

        return builder;
    }
}
