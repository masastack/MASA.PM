// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMasaConfiguration(configurationBuilder => configurationBuilder.UseDcc());

builder.Services.AddMasaIdentity(options =>
{
    options.UserName = "name";
    options.UserId = "sub";
    options.Role = IdentityClaimConsts.ROLES;
    options.Environment = IdentityClaimConsts.ENVIRONMENT;
    options.Mapping(nameof(MasaUser.CurrentTeamId), IdentityClaimConsts.CURRENT_TEAM);
    options.Mapping(nameof(MasaUser.StaffId), IdentityClaimConsts.STAFF);
    options.Mapping(nameof(MasaUser.Account), IdentityClaimConsts.ACCOUNT);
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
    options.Authority = builder.Services.GetMasaConfiguration().Local["IdentityServerUrl"];
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

var redisOptions = AppSettings.GetModel<RedisConfigurationOptions>("RedisConfig");
builder.Services.AddAuthClient(builder.Services.GetMasaConfiguration().Local["AuthServiceBaseAddress"], redisOptions);
builder.Services.AddDccClient(redisOptions);

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
        options.UseDapr()
        .UseUoW<PmDbContext>(dbOptions => dbOptions.UseSqlServer().UseFilter())
        .UseEventLog<PmDbContext>()
        .UseEventBus(eventBusBuilder =>
        {
            eventBusBuilder.UseMiddleware(typeof(DisabledCommandMiddleware<>));
        });
    })
    .AddServices(builder);

await app.MigrateAsync();

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
