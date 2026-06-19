using System;
using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Interfaces.REST.Resources;
using HydroSmart.API.Settings.Domain.Model.ValueObjects;

namespace HydroSmart.API.Settings.Interfaces.REST.Transform;

public static class SettingsResourceFromEntityAssembler
{
    public static SettingsResource ToResourceFromEntity(UserSettings settings)
    {
        var start = DateTime.Today.Add(settings.NotificationScheduleStart).ToString("hh:mm tt");
        var end = DateTime.Today.Add(settings.NotificationScheduleEnd).ToString("hh:mm tt");
        var freq = ReportFrequencyToString(settings.ReportFrequency);
        var format = ReportFormatToString(settings.ReportFormat);

        return new SettingsResource(
            settings.Id,
            settings.UserId,
            settings.CloseValvesWhenActiveForMoreThan50Minutes,
            settings.LockValvesWhenLeavingHome,
            settings.LockValvesWhenExceedingEstimatedConsumption,
            settings.ReduceWaterIntensityDuringOverconsumption,
            settings.HighConsumptionAlertsEnabled,
            settings.DailyWeeklySummaryEnabled,
            start,
            end,
            freq,
            format,
            settings.TwoFactorAuthenticationEnabled);
    }

    private static string ReportFrequencyToString(ReportFrequency freq) => freq switch
    {
        ReportFrequency.Daily => "Diario",
        ReportFrequency.Weekly => "Semanal",
        _ => "Mensual",
    };

    private static string ReportFormatToString(ReportFormat format) => format switch
    {
        ReportFormat.CSV => "CSV",
        _ => "PDF",
    };
}