// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.BuildingBlocks.StackSdks.Auth;
using Masa.Stack.Components.TaskHandle;

namespace MASA.PM.Web.Admin.Components;

public partial class StaffSelect
{
    public StaffSelect()
    {
        _asyncTaskQueue = new AsyncTaskQueue
        {
            AutoCancelPreviousTask = true,
            UseSingleThread = true
        };
    }

    [Parameter]
    public string Class { get; set; } = "";

    [Inject]
    public I18n I18n { get; set; } = default!;

    [Inject]
    IAuthClient AuthClient { get; set; } = default!;

    [Parameter]
    public List<Guid> Value { get; set; } = new();

    [Parameter]
    public EventCallback<List<Guid>> ValueChanged { get; set; }

    [Parameter]
    public List<Guid>? IgnoreValue { get; set; }

    [Parameter]
    public string Label { get; set; } = "";

    [Parameter]
    public bool Readonly { get; set; }

    private bool _staffLoading;
    private List<Guid> _ignoreValue = new();

    List<UserSelectModel> Staffs { get; set; } = new();

    protected override void OnInitialized()
    {
        Label = I18n.T("Staff");
        base.OnInitialized();
    }

    private string lastUserIds = default!;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        var ids = Value != null && Value.Any() ? string.Join(",", Value) : default;
        if (lastUserIds == ids)
            return;
        lastUserIds = ids;
        var data = new List<UserSelectModel>();
        if (!string.IsNullOrEmpty(ids))
        {
            foreach (var userId in Value)
            {
                var user = await AuthClient.UserService.GetByIdAsync(userId);
                if (user != null)
                    data.Add(new UserSelectModel
                    {
                        Account = user.Account,
                        Id = user.Id,
                        Name = user.Name,
                        DisplayName = user.DisplayName,
                        Avatar = user.Avatar,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    });
            }
        }
        Staffs = data;
    }   

    protected override void OnParametersSet()
    {
        if (IgnoreValue?.SequenceEqual(_ignoreValue) == false)
        {
            _ignoreValue = IgnoreValue;
            Staffs.RemoveAll(s => IgnoreValue.Contains(s.Id));
        }

        base.OnParametersSet();
    }

    protected async Task RemoveStaff(UserSelectModel staff)
    {
        if (Readonly is false)
        {
            var value = new List<Guid>();
            value.AddRange(Value);
            value.Remove(staff.Id);
            await UpdateValueAsync(value);
        }
    }

    public async Task UpdateValueAsync(List<Guid> value)
    {
        if (ValueChanged.HasDelegate) await ValueChanged.InvokeAsync(value);
        else Value = value;
    }

    private bool FilterItem(UserSelectModel item, string queryText, string itemText)
    {
        return item.DisplayName.Contains(queryText, StringComparison.OrdinalIgnoreCase);
    }

    private readonly AsyncTaskQueue _asyncTaskQueue;

    private async Task QuerySelectionStaff(string search)
    {
        search = search.TrimStart(' ').TrimEnd(' ');
        if (search.Length == 0)
        {
            Staffs.Clear();
        }
        else
        {
            var result = await _asyncTaskQueue.ExecuteAsync(async () =>
            {
                var response = await AuthClient.UserService.SearchAsync(search);
                return response;
            });
            if (result.IsValid)
            {
                Staffs = result.result;
                StateHasChanged();
            }
        }

    }

    static string ReturnNotNullValue(params string[] values) => values.FirstOrDefault(v => !string.IsNullOrEmpty(v)) ?? "";

    public static string MaskPhoneNumber(string phoneNumber)
    {
        if (phoneNumber.Length == 11)
        {
            return phoneNumber.Substring(0, 3) + "****" + phoneNumber.Substring(7);
        }

        return phoneNumber;
    }
}

