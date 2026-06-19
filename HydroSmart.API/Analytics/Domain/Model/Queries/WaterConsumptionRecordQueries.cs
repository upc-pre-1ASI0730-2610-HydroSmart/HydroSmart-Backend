namespace HydroSmart.API.Analytics.Domain.Model.Queries;

public record GetTodayConsumptionByUserIdQuery(int UserId);
public record GetDailyConsumptionByUserIdQuery(int UserId);
public record GetConsumptionByCategoryQuery(int UserId);
public record GetMonthlyComparisonQuery(int UserId);
public record GetMonthlyTotalAndGoalQuery(int UserId);

// Query result value objects
public record TimeBlockConsumption(string TimeBlock, double Liters);
public record CategoryConsumption(string Category, double Liters);
public record MonthlyConsumption(string Month, double Liters);
public record MonthlyTotalAndGoal(double TotalLiters, double GoalLiters);
