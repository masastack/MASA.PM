// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace MASA.PM.Service.Admin.Infrastructure.Middleware
{
    public class DisabledCommandMiddleware<TEvent> : Middleware<TEvent>
    where TEvent : notnull, IEvent
    {
        readonly IUserContext _userContext;
        readonly IMasaStackConfig _masaStackConfig;

        public DisabledCommandMiddleware(IUserContext userContext, IMasaStackConfig masaStackConfig)
        {
            _userContext = userContext;
            _masaStackConfig = masaStackConfig;
        }

        public override async Task HandleAsync(TEvent @event, EventHandlerDelegate next)
        {
            var user = _userContext.GetUser<MasaUser>();
            if (_masaStackConfig.IsDemo || user?.Account == "Guest" && @event is ICommand)
            {
                throw new UserFriendlyException("演示账号禁止操作");
            }
            await next();
        }
    }
}
