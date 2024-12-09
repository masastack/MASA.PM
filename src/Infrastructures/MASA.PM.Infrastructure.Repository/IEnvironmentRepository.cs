// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Infrastructure.Repository;

public interface IEnvironmentRepository
{
    Task<Domain.Shared.Entities.Environment> AddAsync(Domain.Shared.Entities.Environment environment);

    Task AddEnvironmentClustersAsync(IEnumerable<EnvironmentCluster> environmentClusters);

    Task<EnvironmentCluster> AddEnvironmentClusterAsync(EnvironmentCluster environmentCluster);

    Task<List<Domain.Shared.Entities.Environment>> GetListAsync();

    Task UpdateAsync(UpdateEnvironmentDto model);

    Task<Domain.Shared.Entities.Environment> GetAsync(int Id);

    Task RemoveAsync(int Id);
}
