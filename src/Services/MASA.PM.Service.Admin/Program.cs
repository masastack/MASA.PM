// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

var builder = WebApplication.CreateBuilder(args);

ValidatorOptions.Global.LanguageManager = new MasaLanguageManager();
GlobalValidationOptions.SetDefaultCulture("zh-CN");

await builder.Services.AddMasaStackConfigAsync(MasaStackProject.PM, MasaStackApp.Service);
var masaStackConfig = builder.Services.GetMasaStackConfig();
var connStr = masaStackConfig.GetValue(MasaStackConfigConstant.CONNECTIONSTRING);
var dbModel = JsonSerializer.Deserialize<DbModel>(connStr)!;
bool isPgsql = string.Equals(dbModel.DbType, "postgresql", StringComparison.CurrentCultureIgnoreCase);

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddObservable(builder.Logging, () =>
    {
        return new MasaObservableOptions
        {
            ServiceNameSpace = builder.Environment.EnvironmentName,
            ServiceVersion = masaStackConfig.Version,
            ServiceName = masaStackConfig.GetServiceId(MasaStackProject.PM)
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
    options.UserName = IdentityClaimConsts.USER_NAME;
    options.UserId = IdentityClaimConsts.USER_ID;
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

    options.BackchannelHttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (
            sender,
            certificate,
            chain,
            sslPolicyErrors) =>
        { return true; }
    };
});

#region regist Repository
var repositories = typeof(IAppRepository).Assembly;
var assembly = repositories
    .DefinedTypes
    .Where(a => a.Name.EndsWith("Repository") && !a.Name.StartsWith("I"));
foreach (var item in assembly)
{
    builder.Services.AddScoped(item.GetInterfaces()[0], item);
}
#endregion

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDaprStarter(opt =>
    {
        opt.DaprHttpPort = 3607;
        opt.DaprGrpcPort = 3608;
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
#if DEBUG
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
#endif
    .AddDomainEventBus(options =>
    {
        var connStr = masaStackConfig.GetConnectionString(MasaStackProject.PM.Name);
        if (isPgsql)
            PmDbContext.RegistAssembly(Assembly.Load("MASA.PM.Infrastructure.EFCore.PostgreSql"));
        else
            PmDbContext.RegistAssembly(Assembly.Load("MASA.PM.Infrastructure.EFCore.SqlServer"));
        options.UseIntegrationEventBus(options =>
                                                                options.UseDapr()
                                                                             .UseEventBus())
                    .UseUoW<PmDbContext>(dbOptions =>
                                                                                        (isPgsql ? dbOptions.UsePgsql(connStr, options => options.MigrationsAssembly("MASA.PM.Infrastructure.EFCore.PostgreSql")) :
                                                                                                        dbOptions.UseSqlServer(connStr, options => options.MigrationsAssembly("MASA.PM.Infrastructure.EFCore.SqlServer"))).UseFilter())
                   .UseRepository<PmDbContext>();
    })
    .AddServices(builder, new Assembly[] { typeof(AddAppCommand).Assembly, typeof(AppService).Assembly });

await app.MigrateAsync();
await builder.SeedDataAsync(masaStackConfig);

app.UseMasaExceptionHandler();

// Configure the HTTP request pipeline.
#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
#endif

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
