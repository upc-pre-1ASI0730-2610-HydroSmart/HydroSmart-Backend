namespace HydroSmart.API.Reports.Domain.Model.Commands;

public record UpdateReportCommand(
    int ReportId,
    string Title,
    string Description,
    DateTime Date,
    int Type
);

