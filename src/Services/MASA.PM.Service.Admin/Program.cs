// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

var builder = WebApplication.CreateBuilder(args);

ValidatorOptions.Global.LanguageManager = new MasaLanguageManager();
GlobalValidationOptions.SetDefaultCulture("zh-CN");

await builder.Services.AddMasaStackConfigAsync();
var masaStackConfig = builder.Services.GetMasaStackConfig();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddObservable(builder.Logging, () =>
    {
        return new MasaObservableOptions
        {
            ServiceNameSpace = builder.Environment.EnvironmentName,
            ServiceVersion = masaStackConfig.Version,
            ServiceName = masaStackConfig.GetServerId(MasaStackConstant.PM)
        };
    }, () =>
    {
        return masaStackConfig.OtlpUrl;
    }, true);
}

builder.Services.AddStackMiddleware();
builder.Services.AddI18n(Path.Combine("Assets", "I18n"));

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
    options.Authority = masaStackConfig.GetSsoDomain();
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

var redisModel = masaStackConfig.RedisModel ?? new();
var redisOptions = new RedisConfigurationOptions
{
    Servers = new List<RedisServerOptions>
    {
        new RedisServerOptions(redisModel.RedisHost,redisModel.RedisPort)
    },
    Password = redisModel.RedisPassword,
    DefaultDatabase = redisModel.RedisDb
};
builder.Services
    .AddAuthClient(masaStackConfig.GetAuthServiceDomain(), redisOptions)
    .AddDccClient(redisOptions);

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
    .AddIntegrationEventBus(options =>
    {
        var connStr = masaStackConfig.GetConnectionString(MasaStackConstant.PM);
        options.UseDapr()
        .UseEventLog<PmDbContext>()
        .UseUoW<PmDbContext>(dbOptions => dbOptions.UseSqlServer(connStr).UseFilter())
        .UseEventBus();
    })
    .AddServices(builder);

await app.MigrateAsync();
if (masaStackConfig.IsDemo)
{
    await builder.SeedDataAsync(masaStackConfig);
}

app.UseMasaExceptionHandler();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseStackMiddleware();

app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});
app.UseHttpsRedirection();

app.UseI18n();

app.Run();
