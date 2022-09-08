// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

var builder = WebApplication.CreateBuilder(args);

builder.AddMasaConfiguration(configurationBuilder => configurationBuilder.UseDcc());

builder.Services.AddMasaIdentityModel(options =>
{
    options.Environment = "environment";
    options.UserName = "name";
    options.UserId = "sub";
});

builder.Services.AddDaprClient();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.GetMasaConfiguration().ConfigurationApi.GetDefault().GetValue<string>("Appsettings:IdentityServerUrl");
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.MapInboundClaims = false;
});

#region regist Repository
var repositories = Assembly.GetExecutingAssembly();
var assembly = repositories
    .DefinedTypes
    .Where(a => a.Name.EndsWith("Repository") && !a.Name.StartsWith("I"));
foreach (var item in assembly)
{
    builder.Services.AddScoped(item.GetInterfaces().First(), item);
}
#endregion

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDaprStarter(opt =>
    {
        opt.DaprHttpPort = 3600;
        opt.DaprGrpcPort = 3601;
    });
}

builder.Services.AddMasaIdentityModel(options =>
{
    options.Environment = "environment";
    options.UserName = "name";
    options.UserId = "sub";
});
builder.Services.AddAuthClient(builder.GetMasaConfiguration().ConfigurationApi.GetDefault()["Appsettings:AuthServiceBaseAddress"]);

builder.Services.AddDccClient();

var app = builder.Services
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer xxxxxxxxxxxxxxx\"",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    })
    .AddTransient(typeof(IMiddleware<>), typeof(LogMiddleware<>))
    .AddIntegrationEventBus<IntegrationEventLogService>(options =>
    {
        var connectionString = builder.GetMasaConfiguration().ConfigurationApi.GetDefault()
        ["Appsettings:ConnectionStrings:DefaultConnection"];

        options.UseDapr()
        .UseUoW<PmDbContext>(dbOptions => dbOptions.UseSqlServer(connectionString).UseFilter())
        .UseEventLog<PmDbContext>()
        .UseEventBus();
    })
    .AddServices(builder);

app.UseMasaExceptionHandler();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});
app.UseHttpsRedirection();

app.Run();
