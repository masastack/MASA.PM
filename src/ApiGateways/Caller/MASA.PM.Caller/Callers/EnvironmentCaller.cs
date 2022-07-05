// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.PM.Caller.Callers
{
    public class EnvironmentCaller : HttpClientCallerBase
    {
        private readonly string _prefix = "/api/v1/env";

        public EnvironmentCaller(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Name = nameof(EnvironmentCaller);
        }

        protected override string BaseAddress { get; set; } = AppSettings.Get("ServiceBaseUrl");

        //protected override string AppId { get; set; } = "masa-pm-service-admin";

        public async Task<EnvironmentDetailDto> GetAsync(int Id)
        {
            var result = await CallerProvider.GetAsync<EnvironmentDetailDto>($"{_prefix}/{Id}");

            return result ?? new();
        }

        public async Task InitAsync(InitDto model)
        {
            await CallerProvider.PostAsync($"{_prefix}/init", model);
        }

        public async Task<List<EnvironmentDto>> GetListAsync()
        {
            var result = await CallerProvider.GetAsync<List<EnvironmentDto>>($"{_prefix}");

            return result ?? new();
        }

        public async Task<EnvironmentDto> AddAsync(AddEnvironmentWhitClustersDto model)
        {
            return await CallerProvider.PostAsync<AddEnvironmentWhitClustersDto, EnvironmentDto>(_prefix, model) ?? new();
        }

        public async Task UpdateAsync(UpdateEnvironmentDto model)
        {
            await CallerProvider.PutAsync(_prefix, model);
        }

        public async Task DeleteAsync(int Id)
        {
            await CallerProvider.DeleteAsync(_prefix, Id);
        }
    }
}
