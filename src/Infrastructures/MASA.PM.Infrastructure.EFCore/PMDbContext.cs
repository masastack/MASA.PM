// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.EFCore;

public class PmDbContext : MasaDbContext<PmDbContext>
{
    public PmDbContext(MasaDbContextOptions<PmDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public static Assembly Assembly { get; private set; } = typeof(PmDbContext).Assembly;

    public static void RegistAssembly(Assembly assembly)
    {
        Assembly = assembly;
    }

    protected override void OnModelCreatingExecuting(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly);
        base.OnModelCreatingExecuting(builder);

        //modelBuilder.Entity<Entities.Environment>()
        //    .Property(t => t.ModificationTime)
        //    .HasDefaultValueSql("SYSDATETIME()");

        //modelBuilder.Entity<EnvironmentCluster>();

        //modelBuilder.Entity<Cluster>()
        //    .Property(t => t.ModificationTime)
        //    .HasDefaultValueSql("SYSDATETIME()");

        //modelBuilder.Entity<EnvironmentClusterProject>();

        //modelBuilder.Entity<Project>()
        //    .Property(t => t.ModificationTime)
        //    .HasDefaultValueSql("SYSDATETIME()");

        //modelBuilder.Entity<EnvironmentClusterProjectApp>();

        //modelBuilder.Entity<App>()
        //    .Property(t => t.ModificationTime)
        //    .HasDefaultValueSql("SYSDATETIME()");

        //modelBuilder.Entity<EnvironmentProjectTeam>()
        //    .HasKey(t => new { t.ProjectId, t.TeamId, t.EnvironmentName });

        //modelBuilder.ApplyConfiguration(new IntegrationEventLogEntityTypeConfiguration());
    }

    public DbSet<PM.Infrastructure.Domain.Shared.Entities.Environment> Environments { get; set; } = default!;

    public DbSet<EnvironmentCluster> EnvironmentClusters { get; set; } = default!;

    public DbSet<Cluster> Clusters { get; set; } = default!;

    public DbSet<EnvironmentClusterProject> EnvironmentClusterProjects { get; set; } = default!;

    public DbSet<Project> Projects { get; set; } = default!;

    public DbSet<EnvironmentClusterProjectApp> EnvironmentClusterProjectApps { get; set; } = default!;

    public DbSet<App> Apps { get; set; } = default!;

    public DbSet<EnvironmentProjectTeam> EnvironmentProjectTeams { get; set; } = default!;
}
