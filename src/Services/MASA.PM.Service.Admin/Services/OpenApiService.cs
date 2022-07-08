// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Services
{
    public class OpenApiService : ServiceBase
    {
        public OpenApiService(IServiceCollection services) : base(services)
        {
            App.MapGet("/open-api/project/{identity}", GetProjectByIdentityAsync);
            App.MapGet("/open-api/app/{identity}", GetAppByIdentityAsync);
        }

        public async Task<ProjectDetailDto> GetProjectByIdentityAsync(IEventBus eventBus, string identity)
        {
            var query = new ProjectByIdentityQuery(identity);
            await eventBus.PublishAsync(query);

            return query.Result;
        }

        public async Task<AppDto> GetAppByIdentityAsync(IEventBus eventBus, string identity)
        {
            var query = new AppByIdentityQuery(identity);
            await eventBus.PublishAsync(query);

            return query.Result;
        }
    }
}
