using HydroSmart.API.Settings.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Settings.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySettingsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<UserSettings>().HasKey(settings => settings.Id);
        builder.Entity<UserSettings>().Property(settings => settings.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<UserSettings>().Property(settings => settings.UserId).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.CloseValvesWhenActiveForMoreThan50Minutes).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.LockValvesWhenLeavingHome).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.LockValvesWhenExceedingEstimatedConsumption).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.ReduceWaterIntensityDuringOverconsumption).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.HighConsumptionAlertsEnabled).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.DailyWeeklySummaryEnabled).IsRequired();
        builder.Entity<UserSettings>().Property(settings => settings.NotificationScheduleStart).IsRequired().HasMaxLength(20);
        builder.Entity<UserSettings>().Property(settings => settings.NotificationScheduleEnd).IsRequired().HasMaxLength(20);
        builder.Entity<UserSettings>().Property(settings => settings.ReportFrequency).IsRequired().HasMaxLength(20);
        builder.Entity<UserSettings>().Property(settings => settings.ReportFormat).IsRequired().HasMaxLength(20);
        builder.Entity<UserSettings>().Property(settings => settings.TwoFactorAuthenticationEnabled).IsRequired();
        builder.Entity<UserSettings>().HasIndex(settings => settings.UserId).IsUnique();
    }
}