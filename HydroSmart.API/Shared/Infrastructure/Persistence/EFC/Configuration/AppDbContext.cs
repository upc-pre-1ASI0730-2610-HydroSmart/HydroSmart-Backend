using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using HydroSmart.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Analytics.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Notifications.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Infrastructure.Persistence.EFC.Configuration.Extensions;
using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // IAM Bounded Context
    public DbSet<User> Users { get; set; }
    
    // Profiles Bounded Context
    public DbSet<Profile> Profiles { get; set; }
    
    // Notifications Bounded Context
    public DbSet<Notification> Notifications { get; set; }
    
    // Devices Bounded Context
    public DbSet<Device> Devices { get; set; }
    
    // Analytics Bounded Context
    public DbSet<WaterConsumptionRecord> WaterConsumptionRecords { get; set; }
    
    // Settings Bounded Context
    public DbSet<UserSettings> UserSettings { get; set; }
    
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
        builder.ApplyIamConfiguration();
        
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
        
        // Notifications Context
        builder.ApplyNotificationsConfiguration();
        
        // Subscriptions Context
        //builder.ApplySubscriptionsConfiguration();
        
        // Apply Seed Data
        //builder.ApplySeedData();
        
        builder.UseSnakeCaseNamingConvention();
    }
}