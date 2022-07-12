// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Masa.Contrib.Configuration.ConfigurationApi.Dcc.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprStarter(opt =>
{
    opt.DaprHttpPort = 3600;
    opt.DaprGrpcPort = 3601;
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
    options.Authority = "";
    options.RequireHttpsMetadata = false;
    options.Audience = "";
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

if (builder.Environment.IsProduction())
{
    await AddProductionMasaConfigurationAsync(builder);
}
else
{
    builder.AddMasaConfiguration(configurationBuilder => configurationBuilder.UseDcc());
}

builder.Services.AddMasaIdentityModel(IdentityType.MultiEnvironment, options =>
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
                new string[] {}
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

//SeedData
//await app.Seed();

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

DaprClient GetDaprClient(IServiceCollection services)
{
    var daprClient = services.BuildServiceProvider().GetRequiredService<DaprClient>();

    return daprClient;
}

async Task AddProductionMasaConfigurationAsync(WebApplicationBuilder builder)
{
    var daprClient = GetDaprClient(builder.Services);
    var config = await daprClient.GetSecretAsync("local-secret-store", "Config");
    var redisHost = config["RedisHost"];
    var redisPassword = config["RedisPassword"];
    var redisDB = config["RedisDatabase"];
    var dccAddress = config["DccManageServiceAddress"];
    var dccOptions = new DccConfigurationOptions
    {
        RedisOptions = new Masa.Utils.Caching.Redis.Models.RedisConfigurationOptions
        {
            Servers = new List<Masa.Utils.Caching.Redis.Models.RedisServerOptions>
            {
                new(redisHost)
            },
            DefaultDatabase = int.Parse(redisDB),
            Password = redisPassword
        }
    };
    builder.AddMasaConfiguration(configurationBuilder => configurationBuilder.UseDcc(() => dccOptions, option =>
    {
        option.Environment = builder.Configuration[nameof(DccSectionOptions.Environment)];
        option.Cluster = builder.Configuration[nameof(DccSectionOptions.Cluster)];
        option.AppId = builder.Configuration[nameof(DccSectionOptions.AppId)];
        option.ConfigObjects = builder.Configuration.GetSection(nameof(DccSectionOptions.ConfigObjects)).Get<List<string>>();
        option.Secret = builder.Configuration[nameof(DccSectionOptions.Secret)];
    }, null));
}
