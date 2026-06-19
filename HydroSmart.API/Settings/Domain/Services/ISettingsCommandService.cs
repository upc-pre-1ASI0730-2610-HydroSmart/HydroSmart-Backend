using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Domain.Model.Commands;

namespace HydroSmart.API.Settings.Domain.Services;

public interface ISettingsCommandService
{
    Task<UserSettings?> Handle(CreateSettingsCommand command);
    Task<UserSettings?> Handle(UpdateSettingsCommand command);
}