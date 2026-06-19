namespace HydroSmart.API.Analytics.Domain.Model.Aggregates;

public class WaterConsumptionRecord
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double Liters { get; set; }
    public string Category { get; set; }
    public DateTime RecordedAt { get; set; }

    public WaterConsumptionRecord()
    {
    }

    public WaterConsumptionRecord(int userId, double liters, string category, DateTime? recordedAt = null)
    {
        UserId = userId;
        Liters = liters;
        Category = category;
        RecordedAt = recordedAt.HasValue
            ? DateTime.SpecifyKind(recordedAt.Value, DateTimeKind.Utc)
            : DateTime.UtcNow;
    }
}
