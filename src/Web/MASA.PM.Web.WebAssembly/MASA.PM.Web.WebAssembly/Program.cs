// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton(sp => builder.Configuration);

await builder.Services.AddMasaStackConfigAsync(builder.Configuration);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.RootComponents.Add<WasmRoutes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var masaStackConfig = builder.Services.GetMasaStackConfig();

MasaOpenIdConnectOptions masaOpenIdConnectOptions = new()
{
    Authority = masaStackConfig.GetSsoDomain(),
    ClientId = masaStackConfig.GetWebId(MasaStackProject.PM),
    Scopes = new List<string> { "offline_access" }
};

await builder.AddMasaOpenIdConnectAsync(masaOpenIdConnectOptions);

string pmServiceAddress = masaStackConfig.GetPmServiceDomain();
if (builder.HostEnvironment.IsDevelopment())
{
    pmServiceAddress = "http://localhost:19401";
}

builder.Services.AddPMApiGateways(option =>
{
    option.PMServiceAddress = pmServiceAddress;
    option.AuthorityEndpoint = masaOpenIdConnectOptions.Authority;
    option.ClientId = masaOpenIdConnectOptions.ClientId;
    option.ClientSecret = masaOpenIdConnectOptions.ClientSecret;
});

builder.Services.AddMasaStackComponent(MasaStackProject.PM, $"{builder.HostEnvironment.BaseAddress}i18n");

var host = builder.Build();
await host.Services.InitializeMasaStackApplicationAsync();
await host.RunAsync();
