namespace HydroSmart.API.Settings.Interfaces.REST.Resources;

public record SettingsResource(
    int Id,
    int UserId,
    bool CloseValvesWhenActiveForMoreThan50Minutes,
    bool LockValvesWhenLeavingHome,
    bool LockValvesWhenExceedingEstimatedConsumption,
    bool ReduceWaterIntensityDuringOverconsumption,
    bool HighConsumptionAlertsEnabled,
    bool DailyWeeklySummaryEnabled,
    string NotificationScheduleStart,
    string NotificationScheduleEnd,
    string ReportFrequency,
    string ReportFormat,
    bool TwoFactorAuthenticationEnabled
);