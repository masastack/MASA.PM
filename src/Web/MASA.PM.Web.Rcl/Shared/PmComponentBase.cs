// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Rcl;

public abstract class PmComponentBase : MasaComponentBase
{
    protected Dictionary<Guid, UserModel> _users = null;

    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        if (_users == null) return new();
        if (_users.ContainsKey(userId))
            return _users[userId];
        var user = await AuthClient.UserService.GetByIdAsync(userId);
        _users[userId] = user ?? new();
        return _users[userId];
    }

    protected async Task LoadUsersAsync(params Guid[] userIds)
    {
        foreach (var userId in userIds)
        {
            await GetUserAsync(userId);
        }
    }
}

