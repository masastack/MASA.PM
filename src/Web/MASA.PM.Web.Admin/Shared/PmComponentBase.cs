// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin;

public abstract class PmComponentBase : Masa.Stack.Components.MasaComponentBase
{
    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var user = await AuthClient.UserService.GetByIdAsync(userId);
        return user ?? new();
    }
}

