using HydroSmart.API.Analytics.Domain.Model.Aggregates;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Analytics.Domain.Repositories;

public interface IWaterConsumptionRecordRepository : IBaseRepository<WaterConsumptionRecord>
{
    Task<IEnumerable<WaterConsumptionRecord>> FindByUserIdAsync(int userId);
    Task<IEnumerable<WaterConsumptionRecord>> FindByUserIdAndDateRangeAsync(int userId, DateTime from, DateTime to);
}
