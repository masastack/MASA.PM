// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin;

public abstract class PmComponentBase : Masa.Stack.Components.MasaComponentBase
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

    protected List<UserModel>? GetAppUsers(List<Guid>? userIds)
    {
        if (userIds == null || userIds.Count == 0) return default;
        if (_users == null || _users.Count == 0) return default;
        var result = new List<UserModel>();
        foreach (var userId in userIds)
        {
            if (_users.ContainsKey(userId))
                result.Add(_users[userId]);
        }
        return result;
    }

    protected async Task<Dictionary<int, List<UserModel>>> LoadResponsibilityUsersAsync(List<AppDto> apps)
    {
        var userIds = new List<Guid>();
        if (apps == null || apps.Count == 0)
            return [];

        foreach (var app in apps)
        {
            if (app.ResponsibilityUserIds != null && app.ResponsibilityUserIds.Count > 0)
                userIds.AddRange(app.ResponsibilityUserIds);
        }

        await LoadUsersAsync(userIds.Distinct().ToArray());
        var appUsers = new Dictionary<int, List<UserModel>>();
        foreach (var app in apps)
        {
            if (appUsers.ContainsKey(app.Id))
                continue;
            appUsers.Add(app.Id, GetAppUsers(app.ResponsibilityUserIds)!);
        }
        return appUsers;
    }
}

