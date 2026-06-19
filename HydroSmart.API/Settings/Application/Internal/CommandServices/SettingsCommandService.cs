using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Domain.Model.Commands;
using HydroSmart.API.Settings.Domain.Repositories;
using HydroSmart.API.Settings.Domain.Services;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Settings.Application.Internal.CommandServices;

public class SettingsCommandService(ISettingsRepository settingsRepository, IUnitOfWork unitOfWork) : ISettingsCommandService
{
    public async Task<UserSettings?> Handle(CreateSettingsCommand command)
    {
        var existingSettings = await settingsRepository.FindByUserIdAsync(command.UserId);
        if (existingSettings is not null)
        {
            return null;
        }

        var settings = new UserSettings(
            command.UserId,
            command.CloseValvesWhenActiveForMoreThan50Minutes,
            command.LockValvesWhenLeavingHome,
            command.LockValvesWhenExceedingEstimatedConsumption,
            command.ReduceWaterIntensityDuringOverconsumption,
            command.HighConsumptionAlertsEnabled,
            command.DailyWeeklySummaryEnabled,
            command.NotificationScheduleStart,
            command.NotificationScheduleEnd,
            command.ReportFrequency,
            command.ReportFormat,
            command.TwoFactorAuthenticationEnabled);

        await settingsRepository.AddAsync(settings);
        await unitOfWork.CompleteAsync();
        return settings;
    }

    public async Task<UserSettings?> Handle(UpdateSettingsCommand command)
    {
        var settings = await settingsRepository.FindByIdAsync(command.Id);
        if (settings is null)
        {
            return null;
        }

        settings.UpdatePreferences(
            command.CloseValvesWhenActiveForMoreThan50Minutes,
            command.LockValvesWhenLeavingHome,
            command.LockValvesWhenExceedingEstimatedConsumption,
            command.ReduceWaterIntensityDuringOverconsumption,
            command.HighConsumptionAlertsEnabled,
            command.DailyWeeklySummaryEnabled,
            command.NotificationScheduleStart,
            command.NotificationScheduleEnd,
            command.ReportFrequency,
            command.ReportFormat,
            command.TwoFactorAuthenticationEnabled);

        settingsRepository.Update(settings);
        await unitOfWork.CompleteAsync();
        return settings;
    }
}