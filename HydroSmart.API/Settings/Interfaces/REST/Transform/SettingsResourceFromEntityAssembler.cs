using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Interfaces.REST.Resources;

namespace HydroSmart.API.Settings.Interfaces.REST.Transform;

public static class SettingsResourceFromEntityAssembler
{
    public static SettingsResource ToResourceFromEntity(UserSettings settings)
    {
        return new SettingsResource(
            settings.Id,
            settings.UserId,
            settings.CloseValvesWhenActiveForMoreThan50Minutes,
            settings.LockValvesWhenLeavingHome,
            settings.LockValvesWhenExceedingEstimatedConsumption,
            settings.ReduceWaterIntensityDuringOverconsumption,
            settings.HighConsumptionAlertsEnabled,
            settings.DailyWeeklySummaryEnabled,
            settings.NotificationScheduleStart,
            settings.NotificationScheduleEnd,
            settings.ReportFrequency,
            settings.ReportFormat,
            settings.TwoFactorAuthenticationEnabled);
    }
}