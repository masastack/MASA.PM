// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Web.Admin;

public abstract class PmCompontentBase : ComponentBase
{
    private I18n? _languageProvider;

    [Inject]
    public IAuthClient AuthClient { get; set; } = default!;

    [CascadingParameter]
    public I18n I18n
    {
        get
        {
            return _languageProvider ?? throw new Exception("please Inject I18n!");
        }
        set
        {
            _languageProvider = value;
        }
    }

    public string T(string key)
    {
        return I18n.T(key) ?? "";
    }

    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var user = await AuthClient.UserService.GetByIdAsync(userId);
        return user ?? new();
    }
}

