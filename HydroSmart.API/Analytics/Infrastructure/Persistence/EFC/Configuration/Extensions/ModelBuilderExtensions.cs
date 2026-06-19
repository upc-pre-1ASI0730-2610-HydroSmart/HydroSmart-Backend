using HydroSmart.API.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Analytics.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        // Analytics Context

        builder.Entity<WaterConsumptionRecord>().HasKey(r => r.Id);
        builder.Entity<WaterConsumptionRecord>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<WaterConsumptionRecord>().Property(r => r.UserId).IsRequired();
        builder.Entity<WaterConsumptionRecord>().Property(r => r.Liters).IsRequired();
        builder.Entity<WaterConsumptionRecord>().Property(r => r.Category).IsRequired().HasMaxLength(50);
        builder.Entity<WaterConsumptionRecord>().Property(r => r.RecordedAt).IsRequired();

        builder.Entity<WaterConsumptionRecord>().HasIndex(r => r.UserId);
        builder.Entity<WaterConsumptionRecord>().HasIndex(r => new { r.UserId, r.RecordedAt });
    }
}
