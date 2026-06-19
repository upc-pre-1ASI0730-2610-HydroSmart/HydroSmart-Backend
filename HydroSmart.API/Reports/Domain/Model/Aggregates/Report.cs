namespace HydroSmart.API.Reports.Domain.Model.Aggregates;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int Type { get; set; }
}

public enum ReportType
{
    ConsumptionSummary,
    DevicePerformance,
    CostAnalysis,
    Comparative
}

public enum ReportFormat
{
    Pdf,
    Excel,
    Json
}

public enum ReportStatus
{
    Draft,
    Generated,
    Sent
}

