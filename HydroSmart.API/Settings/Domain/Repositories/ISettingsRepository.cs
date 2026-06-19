using HydroSmart.API.Settings.Domain.Model.Aggregates;

namespace HydroSmart.API.Settings.Domain.Repositories;

public interface ISettingsRepository
{
    Task AddAsync(UserSettings userSettings);
    Task<UserSettings?> FindByIdAsync(int id);
    Task<UserSettings?> FindByUserIdAsync(int userId);
    Task<IEnumerable<UserSettings>> ListAsync();
    void Update(UserSettings userSettings);
}