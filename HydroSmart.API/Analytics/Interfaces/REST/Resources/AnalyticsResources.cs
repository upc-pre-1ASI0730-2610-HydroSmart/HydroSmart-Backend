namespace HydroSmart.API.Analytics.Interfaces.REST.Resources;

public class DashboardResource
{
    public double MonthlyConsumptionLiters { get; set; }
    public double MonthlyGoalLiters { get; set; }
    public double EstimatedSavingsPercentage { get; set; }
    public double TodayConsumptionLiters { get; set; }
    public double EstimatedBill { get; set; }
    public List<TimeBlockResource> DailyConsumption { get; set; } = new();
    public List<CategoryBreakdownResource> CategoryBreakdown { get; set; } = new();
    public List<MonthlyComparisonResource> MonthlyComparison { get; set; } = new();
}

public class TimeBlockResource
{
    public string TimeBlock { get; set; } = string.Empty;
    public double Liters { get; set; }
}

public class CategoryBreakdownResource
{
    public string Category { get; set; } = string.Empty;
    public double Liters { get; set; }
}

public class MonthlyComparisonResource
{
    public string Month { get; set; } = string.Empty;
    public double Liters { get; set; }
}

public class WaterConsumptionRecordResource
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double Liters { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }
}

public class CreateWaterConsumptionRecordRequest
{
    public int UserId { get; set; }
    public double Liters { get; set; }
    public string Category { get; set; } = string.Empty; // Shower | Toilet | Filter | Sink | Other
    public DateTime? RecordedAt { get; set; }
}
