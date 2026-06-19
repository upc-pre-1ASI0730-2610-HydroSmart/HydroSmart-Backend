using HydroSmart.API.Analytics.Domain.Model.Queries;
using HydroSmart.API.Analytics.Domain.Services;
using HydroSmart.API.Analytics.Interfaces.ACL;
using HydroSmart.API.Analytics.Interfaces.REST.Resources;
using HydroSmart.API.Analytics.Interfaces.REST.Transform;

namespace HydroSmart.API.Analytics.Application.ACL;

public class AnalyticsContextFacade : IAnalyticsContextFacade
{
    private const double BillRatePerLiter = 0.012;

    private readonly IWaterConsumptionRecordQueryService _queryService;
    private readonly IWaterConsumptionRecordCommandService _commandService;

    public AnalyticsContextFacade(
        IWaterConsumptionRecordQueryService queryService,
        IWaterConsumptionRecordCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    public async Task<DashboardResource> GetDashboard(int userId)
    {
        var monthlyTotalAndGoal = await _queryService.Handle(new GetMonthlyTotalAndGoalQuery(userId));
        var todayConsumption = await _queryService.Handle(new GetTodayConsumptionByUserIdQuery(userId));
        var dailyConsumption = (await _queryService.Handle(new GetDailyConsumptionByUserIdQuery(userId))).ToList();
        var categoryBreakdown = (await _queryService.Handle(new GetConsumptionByCategoryQuery(userId))).ToList();
        var monthlyComparison = (await _queryService.Handle(new GetMonthlyComparisonQuery(userId))).ToList();

        var currentMonthLiters = monthlyTotalAndGoal.TotalLiters;
        var previousMonthLiters = monthlyComparison.Count >= 2
            ? monthlyComparison[monthlyComparison.Count - 2].Liters
            : 0;

        // Negative = savings (consumed less than previous month)
        var savingsPercentage = previousMonthLiters > 0
            ? Math.Round((currentMonthLiters - previousMonthLiters) / previousMonthLiters * 100, 2)
            : 0;

        return new DashboardResource
        {
            MonthlyConsumptionLiters = currentMonthLiters,
            MonthlyGoalLiters = monthlyTotalAndGoal.GoalLiters,
            EstimatedSavingsPercentage = savingsPercentage,
            TodayConsumptionLiters = todayConsumption,
            EstimatedBill = Math.Round(currentMonthLiters * BillRatePerLiter, 2),
            DailyConsumption = dailyConsumption.Select(TimeBlockResourceFromEntityAssembler.ToResourceFromRecord).ToList(),
            CategoryBreakdown = categoryBreakdown.Select(CategoryBreakdownResourceFromEntityAssembler.ToResourceFromRecord).ToList(),
            MonthlyComparison = monthlyComparison.Select(MonthlyComparisonResourceFromEntityAssembler.ToResourceFromRecord).ToList()
        };
    }

    public async Task<WaterConsumptionRecordResource?> CreateRecord(CreateWaterConsumptionRecordRequest request)
    {
        var command = CreateWaterConsumptionRecordCommandFromResourceAssembler.ToCommandFromResource(request);
        var record = await _commandService.Handle(command);
        return record == null ? null : WaterConsumptionRecordResourceFromEntityAssembler.ToResourceFromEntity(record);
    }
}
