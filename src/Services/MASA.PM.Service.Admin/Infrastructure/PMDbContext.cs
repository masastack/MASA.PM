namespace MASA.PM.Service.Admin.Infrastructure
{
    public class PMDbContext : IntegrationEventLogContext
    {
        public PMDbContext(MasaDbContextOptions<PMDbContext> options) : base(options)
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

            modelBuilder.Entity<EnvironmentCluster>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<Cluster>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<EnvironmentClusterProject>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<Project>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<EnvironmentClusterProjectApp>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<App>()
                .Property(t => t.ModificationTime)
                .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<Team>()
               .Property(t => t.ModificationTime)
               .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<TeamProject>()
               .HasKey(t => new { t.TeamId, t.ProjectId, t.AuthorizationTeamId });
            modelBuilder.Entity<TeamProject>().Property(t => t.ModificationTime)
               .HasDefaultValueSql("SYSDATETIME()");

            modelBuilder.Entity<TeamMember>()
                .HasKey(t => new { t.TeamId, t.UserId });
        }

        public DbSet<Entities.Environment> Environments { get; set; } = default!;

        public DbSet<EnvironmentCluster> EnvironmentClusters { get; set; } = default!;

        public DbSet<Cluster> Clusters { get; set; } = default!;

        public DbSet<EnvironmentClusterProject> EnvironmentClusterProjects { get; set; } = default!;

        public DbSet<Project> Projects { get; set; } = default!;

        public DbSet<EnvironmentClusterProjectApp> EnvironmentClusterProjectApps { get; set; } = default!;

        public DbSet<App> Apps { get; set; } = default!;

        public DbSet<Team> Teams { get; set; } = default!;

        public DbSet<TeamMember> TeamMembers { get; set; } = default!;

        public DbSet<TeamProject> TeamProjects { get; set; } = default!;
    }
}
