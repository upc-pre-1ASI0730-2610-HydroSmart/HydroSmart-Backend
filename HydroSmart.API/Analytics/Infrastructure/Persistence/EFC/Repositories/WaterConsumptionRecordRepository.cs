using HydroSmart.API.Analytics.Domain.Model.Aggregates;
using HydroSmart.API.Analytics.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

public class WaterConsumptionRecordRepository : BaseRepository<WaterConsumptionRecord>, IWaterConsumptionRecordRepository
{
    public WaterConsumptionRecordRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WaterConsumptionRecord>> FindByUserIdAsync(int userId)
    {
        return await Context.Set<WaterConsumptionRecord>()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RecordedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<WaterConsumptionRecord>> FindByUserIdAndDateRangeAsync(int userId, DateTime from, DateTime to)
    {
        return await Context.Set<WaterConsumptionRecord>()
            .Where(r => r.UserId == userId && r.RecordedAt >= from && r.RecordedAt < to)
            .OrderBy(r => r.RecordedAt)
            .ToListAsync();
    }
}
