using HydroSmart.API.Shared.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;

using HydroSmart.API.Profiles.Domain.Repositories;
using HydroSmart.API.Profiles.Domain.Services;
using HydroSmart.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using HydroSmart.API.Profiles.Application.Internal.QueryServices;
using HydroSmart.API.Profiles.Application.Internal.CommandServices;
using HydroSmart.API.Profiles.Interfaces.ACL;
using HydroSmart.API.Profiles.Application.ACL;

using HydroSmart.API.Notifications.Domain.Repositories;
using HydroSmart.API.Notifications.Domain.Services;
using HydroSmart.API.Notifications.Infrastructure.Persistence.EFC.Repositories;
using HydroSmart.API.Notifications.Application.Internal.QueryServices;
using HydroSmart.API.Notifications.Application.Internal.CommandServices;
using HydroSmart.API.Notifications.Interfaces.ACL;
using HydroSmart.API.Notifications.Application.ACL;

using HydroSmart.API.Analytics.Domain.Repositories;
using HydroSmart.API.Analytics.Domain.Services;
using HydroSmart.API.Analytics.Infrastructure.Persistence.EFC.Repositories;
using HydroSmart.API.Analytics.Application.Internal.QueryServices;
using HydroSmart.API.Analytics.Application.Internal.CommandServices;
using HydroSmart.API.Analytics.Interfaces.ACL;
using HydroSmart.API.Analytics.Application.ACL;

using HydroSmart.API.Devices.Application.Internal.CommandServices;
using HydroSmart.API.Devices.Application.Internal.QueryServices;
using HydroSmart.API.Devices.Domain.Repositories;
using HydroSmart.API.Devices.Domain.Services;
using HydroSmart.API.Devices.Infrastructure.Persistence.EFC.Repositories;

using HydroSmart.API.Reports.Infrastructure.Persistence.EFC.Configuration.Extensions;

using HydroSmart.API.Settings.Application.Internal.CommandServices;
using HydroSmart.API.Settings.Application.Internal.QueryServices;
using HydroSmart.API.Settings.Domain.Repositories;
using HydroSmart.API.Settings.Domain.Services;
using HydroSmart.API.Settings.Infrastructure.Persistence.EFC.Repositories;

using HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Components;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpOverrides;

using DotNetEnv;
using System.Text;

// Load environment variables from .env file
var envFile = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
    ? ".env.production"
    : ".env";

if (File.Exists(envFile))
{
    Env.Load(envFile);
}

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
});


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});


var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    allowedOrigins = new[]
    {
        "http://localhost:5173",
        "https://hydrosmartweb.netlify.app"
    };
}

Console.WriteLine($"[CORS] Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"[CORS] Allowed Origins: {string.Join(", ", allowedOrigins)}");

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithExposedHeaders("Authorization");
    });
});


builder.Services.Configure<HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Configuration.TokenSettings>(
    builder.Configuration.GetSection("TokenSettings"));


var tokenSecretFromConfig = builder.Configuration["TokenSettings:Secret"];

if (string.IsNullOrWhiteSpace(tokenSecretFromConfig))
{
    throw new Exception("TokenSettings:Secret is not configured.");
}

var keyBytes = Encoding.ASCII.GetBytes(tokenSecretFromConfig);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrWhiteSpace(connectionStringTemplate))
        {
            connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
        }
    }

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var database = Environment.GetEnvironmentVariable("DB_NAME");

        if (!string.IsNullOrWhiteSpace(host) &&
            !string.IsNullOrWhiteSpace(port) &&
            !string.IsNullOrWhiteSpace(user) &&
            !string.IsNullOrWhiteSpace(database))
        {
            connectionString =
                $"server={host};port={port};user={user};password={password};database={database}";
        }
    }

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception(
            "Database connection string is not set. Configure CONNECTION_STRING, DefaultConnection, or DB_* environment variables.");
    }

    options.UseMySQL(connectionString)
        .LogTo(
            Console.WriteLine,
            builder.Environment.IsDevelopment() ? LogLevel.Information : LogLevel.Error)
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.OrderActionsBy(apiDesc =>
    {
        var controller = apiDesc.ActionDescriptor.RouteValues["controller"];

        return controller switch
        {
            "Devices" => "1-Devices",
            "Profiles" => "2-Profiles",
            "Analytics" => "3-Analytics",
            "Notifications" => "4-Notifications",
            "Settings" => "5-Settings",
            "Reports" => "6-Reports",
            _ => "9-Other"
        };
    });

    options.DocumentFilter<HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions.SwaggerTagOrderDocumentFilter>();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HydroSmart API",
        Version = "v1",
        Description = "HydroSmart API - Smart Water Management",
        Contact = new OpenApiContact
        {
            Name = "HydroSmart Team",
            Email = "contact@hydrosmart.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Profiles Bounded Context
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// Notifications Bounded Context
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();
builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
builder.Services.AddScoped<INotificationsContextFacade, NotificationsContextFacade>();

// Analytics Bounded Context
builder.Services.AddScoped<IWaterConsumptionRecordRepository, WaterConsumptionRecordRepository>();
builder.Services.AddScoped<IWaterConsumptionRecordQueryService, WaterConsumptionRecordQueryService>();
builder.Services.AddScoped<IWaterConsumptionRecordCommandService, WaterConsumptionRecordCommandService>();
builder.Services.AddScoped<IAnalyticsContextFacade, AnalyticsContextFacade>();

// Devices Bounded Context
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceQueryService, DeviceQueryService>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();

// Settings Bounded Context
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ISettingsQueryService, SettingsQueryService>();
builder.Services.AddScoped<ISettingsCommandService, SettingsCommandService>();

// Reports Bounded Context
builder.Services.AddScoped<
    HydroSmart.API.Reports.Domain.Repositories.IReportRepository,
    HydroSmart.API.Reports.Infrastructure.Persistence.EFC.Repositories.ReportRepository>();

builder.Services.AddScoped<
    HydroSmart.API.Reports.Domain.Services.IReportQueryService,
    HydroSmart.API.Reports.Application.Internal.QueryServices.ReportQueryService>();

builder.Services.AddScoped<
    HydroSmart.API.Reports.Domain.Services.IReportCommandService,
    HydroSmart.API.Reports.Application.Internal.CommandServices.ReportCommandService>();

// IAM Bounded Context
builder.Services.AddScoped<
    HydroSmart.API.IAM.Domain.Repositories.IUserRepository,
    HydroSmart.API.IAM.Infrastructure.Persistence.EFC.Repositories.UserRepository>();

builder.Services.AddScoped<
    HydroSmart.API.IAM.Domain.Services.IUserQueryService,
    HydroSmart.API.IAM.Application.Internal.QueryServices.UserQueryService>();

builder.Services.AddScoped<
    HydroSmart.API.IAM.Domain.Services.IUserCommandService,
    HydroSmart.API.IAM.Application.Internal.CommandServices.UserCommandService>();

builder.Services.AddScoped<
    HydroSmart.API.IAM.Application.Internal.OutboundServices.ITokenService,
    HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Services.TokenService>();

builder.Services.AddScoped<
    HydroSmart.API.IAM.Application.Internal.OutboundServices.IHashingService,
    HydroSmart.API.IAM.Infrastructure.Hashing.BCrypt.Services.HashingService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
    await context.EnsureReportsSchemaAsync();
}

app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS must be before Authentication, Authorization and custom middleware
app.UseCors("DefaultCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestAuthorizationMiddleware>();

app.MapControllers();

app.Run();