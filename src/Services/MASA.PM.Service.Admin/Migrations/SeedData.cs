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

            context.SaveChanges();
        }
    }
}
