using HydroSmart.API.Settings.Domain.Model.Commands;
using HydroSmart.API.Settings.Interfaces.REST.Resources;

namespace HydroSmart.API.Settings.Interfaces.REST.Transform;

public static class CreateSettingsCommandFromResourceAssembler
{
    public static CreateSettingsCommand ToCommandFromResource(CreateSettingsResource resource)
    {
        return new CreateSettingsCommand(
            resource.UserId,
            resource.CloseValvesWhenActiveForMoreThan50Minutes,
            resource.LockValvesWhenLeavingHome,
            resource.LockValvesWhenExceedingEstimatedConsumption,
            resource.ReduceWaterIntensityDuringOverconsumption,
            resource.HighConsumptionAlertsEnabled,
            resource.DailyWeeklySummaryEnabled,
            resource.NotificationScheduleStart,
            resource.NotificationScheduleEnd,
            resource.ReportFrequency,
            resource.ReportFormat,
            resource.TwoFactorAuthenticationEnabled);
    }
}