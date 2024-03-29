﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.Contrib.Configuration.ConfigurationApi.Dcc;

var builder = WebApplication.CreateBuilder(args);

ValidatorOptions.Global.LanguageManager = new MasaLanguageManager();
GlobalValidationOptions.SetDefaultCulture("zh-CN");

builder.Services.AddHttpContextAccessor();
await builder.Services.AddMasaStackComponentsAsync(MasaStackProject.PM);
var masaStackConfig = builder.Services.GetMasaStackConfig();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

var masaConfiguration = builder.Services.GetMasaConfiguration(); 
builder.Services.AddDocs(masaConfiguration.ConfigurationApi.GetDefault());

if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseKestrel(option =>
    {
        option.ConfigureHttpsDefaults(options =>
        {
            options.ServerCertificate = X509Certificate2.CreateFromPemFile("./ssl/tls.crt", "./ssl/tls.key");
            options.CheckCertificateRevocation = false;
        });
    });
}

MasaOpenIdConnectOptions masaOpenIdConnectOptions = new MasaOpenIdConnectOptions
{
    Authority = masaStackConfig.GetSsoDomain(),
    ClientId = masaStackConfig.GetWebId(MasaStackProject.PM),
    Scopes = new List<string> { "offline_access" }
};

string pmServiceAddress = masaStackConfig.GetPmServiceDomain();
if (builder.Environment.IsDevelopment())
{
    pmServiceAddress = "http://localhost:19401";
}

IdentityModelEventSource.ShowPII = true;
builder.Services.AddMasaOpenIdConnect(masaOpenIdConnectOptions);

builder.Services.AddPMApiGateways(option =>
{
    option.PMServiceAddress = pmServiceAddress;
    option.AuthorityEndpoint = masaOpenIdConnectOptions.Authority;
    option.ClientId = masaOpenIdConnectOptions.ClientId;
    option.ClientSecret = masaOpenIdConnectOptions.ClientSecret;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
