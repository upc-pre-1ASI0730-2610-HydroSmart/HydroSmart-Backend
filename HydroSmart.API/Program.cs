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
using HydroSmart.API.Settings.Application.Internal.CommandServices;
using HydroSmart.API.Settings.Application.Internal.QueryServices;
using HydroSmart.API.Settings.Domain.Repositories;
using HydroSmart.API.Settings.Domain.Services;
using HydroSmart.API.Settings.Infrastructure.Persistence.EFC.Repositories;
using HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Configure TokenSettings
builder.Services.Configure<HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Configuration.TokenSettings>(
    builder.Configuration.GetSection("TokenSettings"));

// Configure JWT Authentication
var tokenSecretFromConfig = builder.Configuration["TokenSettings:Secret"]; 
if (!string.IsNullOrEmpty(tokenSecretFromConfig))
{
    var keyBytes = Encoding.ASCII.GetBytes(tokenSecretFromConfig);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
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
}

// Configure Database Connection based on Environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionStringTemplate))
        {
            // Fallback to InMemory if no connection string is configured
            options.UseInMemoryDatabase("HydroSmart.Profiles.Dev");
        }
        else
        {
            var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Database connection string is not set in the configuration.");
            }
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    });
}
else if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        // First, try to use CONNECTION_STRING directly if available
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        // If not available, build from individual environment variables or configuration
        if (string.IsNullOrEmpty(connectionString))
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionStringTemplate = configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrEmpty(connectionStringTemplate))
            {
                connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
            }
            else
            {
                // Build connection string from individual environment variables
                var host = Environment.GetEnvironmentVariable("DB_HOST");
                var port = Environment.GetEnvironmentVariable("DB_PORT");
                var user = Environment.GetEnvironmentVariable("DB_USER");
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
                var database = Environment.GetEnvironmentVariable("DB_NAME");

                if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(port) &&
                    !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(database))
                {
                    connectionString = $"server={host};port={port};user={user};password={password};database={database}";
                }
            }
        }

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database connection string is not set. Please configure CONNECTION_STRING or individual DB_* environment variables.");

        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });
}

// Add CORS Policy (configurable)
var allowedOriginsConfig = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
var allowedOrigins = allowedOriginsConfig != null && allowedOriginsConfig.Length > 0 && allowedOriginsConfig[0].Contains("%")
    ? Environment.ExpandEnvironmentVariables(allowedOriginsConfig[0]).Split(',', StringSplitOptions.RemoveEmptyEntries)
    : allowedOriginsConfig;

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Development: allow all origins to simplify local testing
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else if (allowedOrigins != null && allowedOrigins.Length > 0)
        {
            // Production: only allow configured origins
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            // Fallback: allow any method/header
            policy.AllowAnyMethod().AllowAnyHeader();
        }
    });
});
// Configure Database Connection based on Environment
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionStringTemplate))
            throw new Exception("Database connection string template is not set in the configuration.");
        var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database connection string is not set in the configuration.");
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        // First, try to use CONNECTION_STRING directly if available
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        
        // If not available, build from individual environment variables or configuration
        if (string.IsNullOrEmpty(connectionString))
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            var connectionStringTemplate = configuration.GetConnectionString("DefaultConnection");
            
            if (!string.IsNullOrEmpty(connectionStringTemplate))
            {
                connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
            }
            else
            {
                // Build connection string from individual environment variables
                var host = Environment.GetEnvironmentVariable("DB_HOST");
                var port = Environment.GetEnvironmentVariable("DB_PORT");
                var user = Environment.GetEnvironmentVariable("DB_USER");
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
                var database = Environment.GetEnvironmentVariable("DB_NAME");
                
                if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(port) && 
                    !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(database))
                {
                    connectionString = $"server={host};port={port};user={user};password={password};database={database}";
                }
            }
        }
        
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database connection string is not set. Please configure CONNECTION_STRING or individual DB_* environment variables.");
        
        Console.WriteLine($"Using connection string: server={connectionString.Split(';')[0].Split('=')[1]};port={connectionString.Split(';')[1].Split('=')[1]};database={connectionString.Split(';')[4].Split('=')[1]}");
        
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error)
            .EnableDetailedErrors();
    });

// Swagger Configuration
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
            _ => "9-Other"
        };
    });
    options.DocumentFilter<HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions.SwaggerTagOrderDocumentFilter>();
    options.SwaggerDoc("v1",
        new OpenApiInfo
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

// ========================
// Dependency Injection
// ========================

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

<<<<<<< HEAD
// IAM Bounded Context
builder.Services.AddScoped<HydroSmart.API.IAM.Domain.Repositories.IUserRepository, HydroSmart.API.IAM.Infrastructure.Persistence.EFC.Repositories.UserRepository>();
builder.Services.AddScoped<HydroSmart.API.IAM.Domain.Services.IUserQueryService, HydroSmart.API.IAM.Application.Internal.QueryServices.UserQueryService>();
builder.Services.AddScoped<HydroSmart.API.IAM.Domain.Services.IUserCommandService, HydroSmart.API.IAM.Application.Internal.CommandServices.UserCommandService>();
builder.Services.AddScoped<HydroSmart.API.IAM.Application.Internal.OutboundServices.ITokenService, HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Services.TokenService>();
builder.Services.AddScoped<HydroSmart.API.IAM.Application.Internal.OutboundServices.IHashingService, HydroSmart.API.IAM.Infrastructure.Hashing.BCrypt.Services.HashingService>();
=======
// Settings Bounded Context
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ISettingsQueryService, SettingsQueryService>();
builder.Services.AddScoped<ISettingsCommandService, SettingsCommandService>();

// ========================
>>>>>>> ce27fa6 (feat (settings): add settings backend part.)

var app = builder.Build();

// Verify if the database exists and create it if it doesn't (development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

// Apply CORS Policy
app.UseCors("DefaultCorsPolicy");

app.UseHttpsRedirection();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Add Request Authorization Middleware for Token Validation
app.UseMiddleware<RequestAuthorizationMiddleware>();

app.MapControllers();

app.Run();

