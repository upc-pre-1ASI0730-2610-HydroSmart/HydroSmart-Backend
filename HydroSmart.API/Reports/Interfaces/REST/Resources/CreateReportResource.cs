namespace HydroSmart.API.Reports.Interfaces.REST.Resources;

public class CreateReportResource
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int Type { get; set; }
}

