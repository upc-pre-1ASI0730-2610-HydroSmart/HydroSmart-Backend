using HydroSmart.API.Analytics.Domain.Model.Queries;
using HydroSmart.API.Analytics.Domain.Repositories;
using HydroSmart.API.Analytics.Domain.Services;

namespace HydroSmart.API.Analytics.Application.Internal.QueryServices;

public class WaterConsumptionRecordQueryService : IWaterConsumptionRecordQueryService
{
    private const double MonthlyGoalLiters = 8000.0;

    private static readonly (string Label, int StartHour, int EndHour)[] TimeBlocks =
    {
        ("00:00-05:00", 0, 5),
        ("05:00-10:00", 5, 10),
        ("10:00-14:00", 10, 14),
        ("14:00-18:00", 14, 18),
        ("18:00-22:00", 18, 22),
        ("22:00-00:00", 22, 24),
    };

    private readonly IWaterConsumptionRecordRepository _repository;

    public WaterConsumptionRecordQueryService(IWaterConsumptionRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<double> Handle(GetTodayConsumptionByUserIdQuery query)
    {
        var today = DateTime.UtcNow.Date;
        var records = await _repository.FindByUserIdAndDateRangeAsync(query.UserId, today, today.AddDays(1));
        return records.Sum(r => r.Liters);
    }

    public async Task<IEnumerable<TimeBlockConsumption>> Handle(GetDailyConsumptionByUserIdQuery query)
    {
        var today = DateTime.UtcNow.Date;
        var records = (await _repository.FindByUserIdAndDateRangeAsync(query.UserId, today, today.AddDays(1))).ToList();

        return TimeBlocks.Select(block => new TimeBlockConsumption(
            block.Label,
            records
                .Where(r => r.RecordedAt.Hour >= block.StartHour && r.RecordedAt.Hour < block.EndHour)
                .Sum(r => r.Liters)
        ));
    }

    public async Task<IEnumerable<CategoryConsumption>> Handle(GetConsumptionByCategoryQuery query)
    {
        var records = await _repository.FindByUserIdAsync(query.UserId);
        return records
            .GroupBy(r => r.Category)
            .Select(g => new CategoryConsumption(g.Key, g.Sum(r => r.Liters)))
            .OrderByDescending(c => c.Liters);
    }

    public async Task<IEnumerable<MonthlyConsumption>> Handle(GetMonthlyComparisonQuery query)
    {
        var now = DateTime.UtcNow;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var from = currentMonthStart.AddMonths(-3);
        var to = currentMonthStart.AddMonths(1);

        var records = (await _repository.FindByUserIdAndDateRangeAsync(query.UserId, from, to)).ToList();

        return Enumerable.Range(-3, 4)
            .Select(offset => now.AddMonths(offset))
            .Select(m => new MonthlyConsumption(
                m.ToString("MMM yyyy"),
                records
                    .Where(r => r.RecordedAt.Year == m.Year && r.RecordedAt.Month == m.Month)
                    .Sum(r => r.Liters)
            ));
    }

    public async Task<MonthlyTotalAndGoal> Handle(GetMonthlyTotalAndGoalQuery query)
    {
        var now = DateTime.UtcNow;
        var from = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var records = await _repository.FindByUserIdAndDateRangeAsync(query.UserId, from, from.AddMonths(1));
        return new MonthlyTotalAndGoal(records.Sum(r => r.Liters), MonthlyGoalLiters);
    }
}
