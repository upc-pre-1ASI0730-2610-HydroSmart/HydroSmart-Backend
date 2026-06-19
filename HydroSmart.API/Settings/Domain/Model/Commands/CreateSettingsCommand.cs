using System;
using HydroSmart.API.Settings.Domain.Model.ValueObjects;

namespace HydroSmart.API.Settings.Domain.Model.Commands;

public record CreateSettingsCommand(
    int UserId,
    bool CloseValvesWhenActiveForMoreThan50Minutes,
    bool LockValvesWhenLeavingHome,
    bool LockValvesWhenExceedingEstimatedConsumption,
    bool ReduceWaterIntensityDuringOverconsumption,
    bool HighConsumptionAlertsEnabled,
    bool DailyWeeklySummaryEnabled,
    TimeSpan NotificationScheduleStart,
    TimeSpan NotificationScheduleEnd,
    ReportFrequency ReportFrequency,
    ReportFormat ReportFormat,
    bool TwoFactorAuthenticationEnabled
);