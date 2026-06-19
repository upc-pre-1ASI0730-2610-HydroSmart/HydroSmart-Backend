using System;
using HydroSmart.API.Settings.Domain.Model.Commands;
using HydroSmart.API.Settings.Interfaces.REST.Resources;
using HydroSmart.API.Settings.Domain.Model.ValueObjects;

namespace HydroSmart.API.Settings.Interfaces.REST.Transform;

public static class UpdateSettingsCommandFromResourceAssembler
{
    public static UpdateSettingsCommand ToCommandFromResource(UpdateSettingsResource resource, int settingsId)
    {
        var start = ParseTimeSpan(resource.NotificationScheduleStart);
        var end = ParseTimeSpan(resource.NotificationScheduleEnd);
        var freq = ParseReportFrequency(resource.ReportFrequency);
        var format = ParseReportFormat(resource.ReportFormat);

        return new UpdateSettingsCommand(
            settingsId,
            resource.CloseValvesWhenActiveForMoreThan50Minutes,
            resource.LockValvesWhenLeavingHome,
            resource.LockValvesWhenExceedingEstimatedConsumption,
            resource.ReduceWaterIntensityDuringOverconsumption,
            resource.HighConsumptionAlertsEnabled,
            resource.DailyWeeklySummaryEnabled,
            start,
            end,
            freq,
            format,
            resource.TwoFactorAuthenticationEnabled);
    }

    private static TimeSpan ParseTimeSpan(string value)
    {
        if (TimeSpan.TryParse(value, out var ts)) return ts;
        if (DateTime.TryParse(value, out var dt)) return dt.TimeOfDay;
        return TimeSpan.Zero;
    }

    private static ReportFrequency ParseReportFrequency(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return ReportFrequency.Monthly;
        value = value.Trim().ToLowerInvariant();
        return value switch
        {
            "diario" or "daily" => ReportFrequency.Daily,
            "semanal" or "weekly" => ReportFrequency.Weekly,
            _ => ReportFrequency.Monthly,
        };
    }

    private static ReportFormat ParseReportFormat(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return ReportFormat.PDF;
        value = value.Trim().ToLowerInvariant();
        return value switch
        {
            "csv" => ReportFormat.CSV,
            _ => ReportFormat.PDF,
        };
    }
}