using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Domain.Model.Queries;
using HydroSmart.API.Settings.Domain.Repositories;
using HydroSmart.API.Settings.Domain.Services;

namespace HydroSmart.API.Settings.Application.Internal.QueryServices;

public class SettingsQueryService(ISettingsRepository settingsRepository) : ISettingsQueryService
{
    public async Task<IEnumerable<UserSettings>> Handle(GetAllSettingsQuery query)
    {
        return await settingsRepository.ListAsync();
    }

    public async Task<UserSettings?> Handle(GetSettingsByIdQuery query)
    {
        return await settingsRepository.FindByIdAsync(query.SettingsId);
    }

    public async Task<UserSettings?> Handle(GetSettingsByUserIdQuery query)
    {
        return await settingsRepository.FindByUserIdAsync(query.UserId);
    }
}