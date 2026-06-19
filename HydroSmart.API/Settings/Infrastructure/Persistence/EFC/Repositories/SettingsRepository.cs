using HydroSmart.API.Settings.Domain.Model.Aggregates;
using HydroSmart.API.Settings.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Settings.Infrastructure.Persistence.EFC.Repositories;

public class SettingsRepository(AppDbContext context) : BaseRepository<UserSettings>(context), ISettingsRepository
{
    public async Task<UserSettings?> FindByUserIdAsync(int userId)
    {
        return await Context.Set<UserSettings>().FirstOrDefaultAsync(settings => settings.UserId == userId);
    }
}