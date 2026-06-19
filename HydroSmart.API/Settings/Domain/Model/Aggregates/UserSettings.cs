using System;
using HydroSmart.API.Settings.Domain.Model.ValueObjects;

namespace HydroSmart.API.Settings.Domain.Model.Aggregates;

public partial class UserSettings
{
    public int Id { get; }
    public int UserId { get; private set; }
    public bool CloseValvesWhenActiveForMoreThan50Minutes { get; private set; }
    public bool LockValvesWhenLeavingHome { get; private set; }
    public bool LockValvesWhenExceedingEstimatedConsumption { get; private set; }
    public bool ReduceWaterIntensityDuringOverconsumption { get; private set; }
    public bool HighConsumptionAlertsEnabled { get; private set; }
    public bool DailyWeeklySummaryEnabled { get; private set; }
    public TimeSpan NotificationScheduleStart { get; private set; }
    public TimeSpan NotificationScheduleEnd { get; private set; }
    public ReportFrequency ReportFrequency { get; private set; }
    public ReportFormat ReportFormat { get; private set; }
    public bool TwoFactorAuthenticationEnabled { get; private set; }

    public UserSettings()
    {
        NotificationScheduleStart = TimeSpan.Zero;
        NotificationScheduleEnd = TimeSpan.Zero;
        ReportFrequency = ReportFrequency.Monthly;
        ReportFormat = ReportFormat.PDF;
    }

    public UserSettings(
        int userId,
        bool closeValvesWhenActiveForMoreThan50Minutes,
        bool lockValvesWhenLeavingHome,
        bool lockValvesWhenExceedingEstimatedConsumption,
        bool reduceWaterIntensityDuringOverconsumption,
        bool highConsumptionAlertsEnabled,
        bool dailyWeeklySummaryEnabled,
        TimeSpan notificationScheduleStart,
        TimeSpan notificationScheduleEnd,
        ReportFrequency reportFrequency,
        ReportFormat reportFormat,
        bool twoFactorAuthenticationEnabled)
    {
        UserId = userId;
        CloseValvesWhenActiveForMoreThan50Minutes = closeValvesWhenActiveForMoreThan50Minutes;
        LockValvesWhenLeavingHome = lockValvesWhenLeavingHome;
        LockValvesWhenExceedingEstimatedConsumption = lockValvesWhenExceedingEstimatedConsumption;
        ReduceWaterIntensityDuringOverconsumption = reduceWaterIntensityDuringOverconsumption;
        HighConsumptionAlertsEnabled = highConsumptionAlertsEnabled;
        DailyWeeklySummaryEnabled = dailyWeeklySummaryEnabled;
        NotificationScheduleStart = notificationScheduleStart;
        NotificationScheduleEnd = notificationScheduleEnd;
        ReportFrequency = reportFrequency;
        ReportFormat = reportFormat;
        TwoFactorAuthenticationEnabled = twoFactorAuthenticationEnabled;
    }

    public UserSettings UpdatePreferences(
        bool closeValvesWhenActiveForMoreThan50Minutes,
        bool lockValvesWhenLeavingHome,
        bool lockValvesWhenExceedingEstimatedConsumption,
        bool reduceWaterIntensityDuringOverconsumption,
        bool highConsumptionAlertsEnabled,
        bool dailyWeeklySummaryEnabled,
        TimeSpan notificationScheduleStart,
        TimeSpan notificationScheduleEnd,
        ReportFrequency reportFrequency,
        ReportFormat reportFormat,
        bool twoFactorAuthenticationEnabled)
    {
        CloseValvesWhenActiveForMoreThan50Minutes = closeValvesWhenActiveForMoreThan50Minutes;
        LockValvesWhenLeavingHome = lockValvesWhenLeavingHome;
        LockValvesWhenExceedingEstimatedConsumption = lockValvesWhenExceedingEstimatedConsumption;
        ReduceWaterIntensityDuringOverconsumption = reduceWaterIntensityDuringOverconsumption;
        HighConsumptionAlertsEnabled = highConsumptionAlertsEnabled;
        DailyWeeklySummaryEnabled = dailyWeeklySummaryEnabled;
        NotificationScheduleStart = notificationScheduleStart;
        NotificationScheduleEnd = notificationScheduleEnd;
        ReportFrequency = reportFrequency;
        ReportFormat = reportFormat;
        TwoFactorAuthenticationEnabled = twoFactorAuthenticationEnabled;
        return this;
    }
}