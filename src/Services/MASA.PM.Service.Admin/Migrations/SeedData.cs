namespace MASA.PM.Service.Admin.Migrations
{
    public static class SeedData
    {
        public static async Task Seed(this IHost host)
        {
            await using var scope = host.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<PMDbContext>();

            if (context.Labels.Any())
            {
                return;
            }

            var now = DateTime.Now;
            var projectTypes = new List<Label>
            {
                new Label
                {
                    Name="Basic Ability",
                    Creator = MasaUser.UserId,
                    Modifier = MasaUser.UserId,
                    CreationTime = now,
                    ModificationTime = now
                },
                new Label
                {
                    Name="Ops Ability",
                    Creator = MasaUser.UserId,
                    Modifier = MasaUser.UserId,
                    CreationTime = now,
                    ModificationTime = now
                },
                new Label
                {
                    Name="Data Factory",
                    Creator = MasaUser.UserId,
                    Modifier = MasaUser.UserId,
                    CreationTime = now,
                    ModificationTime = now
                }
            };

            context.Labels.AddRange(projectTypes);
            context.SaveChanges();
        }
    }
}
