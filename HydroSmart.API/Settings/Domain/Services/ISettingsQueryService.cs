using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Domain.Model.Queries;

namespace HydroSmart.API.Settings.Domain.Services;

public interface ISettingsQueryService
{
    Task<IEnumerable<UserSettings>> Handle(GetAllSettingsQuery query);
    Task<UserSettings?> Handle(GetSettingsByIdQuery query);
    Task<UserSettings?> Handle(GetSettingsByUserIdQuery query);
}