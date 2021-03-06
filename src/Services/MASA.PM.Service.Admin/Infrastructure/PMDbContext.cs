// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.Contrib.Data.EntityFrameworkCore;

namespace MASA.PM.Service.Admin.Infrastructure
{
    public class PmDbContext : MasaDbContext
    {
        public PmDbContext(MasaDbContextOptions<PmDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreatingExecuting(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Environment>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<EnvironmentCluster>();

            modelBuilder.Entity<Cluster>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<EnvironmentClusterProject>();

            modelBuilder.Entity<Project>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<EnvironmentClusterProjectApp>();

            modelBuilder.Entity<App>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");
        }

        public DbSet<Entities.Environment> Environments { get; set; } = default!;

        public DbSet<EnvironmentCluster> EnvironmentClusters { get; set; } = default!;

        public DbSet<Cluster> Clusters { get; set; } = default!;

        public DbSet<EnvironmentClusterProject> EnvironmentClusterProjects { get; set; } = default!;

        public DbSet<Project> Projects { get; set; } = default!;

        public DbSet<EnvironmentClusterProjectApp> EnvironmentClusterProjectApps { get; set; } = default!;

        public DbSet<App> Apps { get; set; } = default!;
    }
}
