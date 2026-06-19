namespace HydroSmart.API.Reports.Domain.Model.Commands;

public record CreateReportCommand(
    string Title,
    string Description,
    DateTime Date,
    int Type
);

