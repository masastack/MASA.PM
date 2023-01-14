// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMasaStackConfig();
var masaStackConfig = builder.Services.GetMasaStackConfig();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddObservable(builder.Logging, () =>
    {
        return new MasaObservableOptions
        {
            ServiceNameSpace = builder.Environment.EnvironmentName,
            ServiceVersion = masaStackConfig.Version,
            ServiceName = masaStackConfig.GetUiId("pm")
        };
    }, () =>
    {
        return masaStackConfig.OtlpUrl;
    }, true);
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddScoped<TokenProvider>();

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.WebHost.UseKestrel(option =>
{
    option.ConfigureHttpsDefaults(options =>
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TLS_NAME")))
        {
            options.ServerCertificate = new X509Certificate2(Path.Combine("Certificates", "7348307__lonsid.cn.pfx"), "cqUza0MN");
        }
        else
        {
            options.ServerCertificate = X509Certificate2.CreateFromPemFile("./ssl/tls.crt", "./ssl/tls.key");
        }
        options.CheckCertificateRevocation = false;
    });
});

builder.AddMasaStackComponentsForServer("wwwroot/i18n");

MasaOpenIdConnectOptions masaOpenIdConnectOptions = new MasaOpenIdConnectOptions
{
    Authority = masaStackConfig.GetSsoDomain(),
    ClientId = masaStackConfig.GetUiId("pm"),
    Scopes = new List<string> { "offline_access" }
};

IdentityModelEventSource.ShowPII = true;

builder.Services.AddMasaOpenIdConnect(masaOpenIdConnectOptions);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<HttpClientAuthorizationDelegatingHandler>();

builder.Services.AddPMApiGateways(c => c.PMServiceAddress = masaStackConfig.GetPmServiceDomain());

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
