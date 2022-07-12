// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.IRepositories
{
    public interface IEnvironmentRepository
    {
        Task<Entities.Environment> AddAsync(Entities.Environment environment);

        Task AddEnvironmentClustersAsync(IEnumerable<EnvironmentCluster> environmentClusters);

        Task<EnvironmentCluster> AddEnvironmentClusterAsync(EnvironmentCluster environmentCluster);

        Task<List<Entities.Environment>> GetListAsync();

        Task UpdateAsync(UpdateEnvironmentDto model);

        Task<Entities.Environment> GetAsync(int Id);

        Task RemoveAsync(int Id);
    }
}
