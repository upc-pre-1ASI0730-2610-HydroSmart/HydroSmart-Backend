namespace HydroSmart.API.Settings.Domain.Model.Commands;

public record CreateSettingsCommand(
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