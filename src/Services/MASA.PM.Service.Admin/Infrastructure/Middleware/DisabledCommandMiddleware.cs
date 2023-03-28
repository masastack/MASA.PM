// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.BuildingBlocks.Globalization.I18n;

namespace MASA.PM.Service.Admin.Infrastructure.Middleware
{
    public class DisabledCommandMiddleware<TEvent> : EventMiddleware<TEvent>
    where TEvent : notnull, IEvent
    {
        readonly IUserContext _userContext;
        readonly IMasaStackConfig _masaStackConfig;
        readonly II18n<DefaultResource> _i18N;

        public DisabledCommandMiddleware(IUserContext userContext, IMasaStackConfig masaStackConfig, II18n<DefaultResource> i18N)
        {
            _userContext = userContext;
            _masaStackConfig = masaStackConfig;
            _i18N = i18N;
        }

        public override async Task HandleAsync(TEvent @event, EventHandlerDelegate next)
        {
            var user = _userContext.GetUser<MasaUser>();
            if (_masaStackConfig.IsDemo && user?.Account == "guest" && @event is ICommand)
            {
                throw new UserFriendlyException(_i18N.T("Demo Account Prohibited Operations"));
            }
            await next();
        }
    }
}
