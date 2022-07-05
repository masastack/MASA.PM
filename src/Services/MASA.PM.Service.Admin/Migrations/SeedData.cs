// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Migrations
{
    public static class SeedData
    {
        public static async Task Seed(this IHost host)
        {
            await using var scope = host.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<PmDbContext>();

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
                    TypeCode ="ProjectType",
                    TypeName ="项目类型",
                    Creator = MasaUser.UserId,
                    Modifier = MasaUser.UserId,
                    CreationTime = now,
                    ModificationTime = now
                },
                new Label
                {
                    Name="Ops Ability",
                    TypeCode ="ProjectType",
                    TypeName ="项目类型",
                    Creator = MasaUser.UserId,
                    Modifier = MasaUser.UserId,
                    CreationTime = now,
                    ModificationTime = now
                },
                new Label
                {
                    Name="Data Factory",
                    TypeCode ="ProjectType",
                    TypeName ="项目类型",
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
