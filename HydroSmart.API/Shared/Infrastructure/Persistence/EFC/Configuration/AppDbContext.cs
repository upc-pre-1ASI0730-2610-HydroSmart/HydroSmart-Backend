using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using HydroSmart.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Settings.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Devices.Infrastructure.Persistence.EFC.Configuration.Extensions;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        // Add the created and updated interceptor
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // IAM Context
        //builder.ApplyIamConfiguration();
        
        // Profiles Context
        builder.ApplyProfilesConfiguration();

        // Settings Context
        builder.ApplySettingsConfiguration();

        // Analytics Context
        builder.ApplyAnalyticsConfiguration();

        // Projects Context

        //add more
        
        // Devices Context
        builder.ApplyDevicesConfiguration();
        
        // Subscriptions Context
        //builder.ApplySubscriptionsConfiguration();
        
        // Apply Seed Data
        //builder.ApplySeedData();
        
        builder.UseSnakeCaseNamingConvention();
    }
}