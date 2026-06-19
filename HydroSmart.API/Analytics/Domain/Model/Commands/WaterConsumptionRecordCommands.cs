namespace HydroSmart.API.Analytics.Domain.Model.Commands;

public record CreateWaterConsumptionRecordCommand(
    int UserId,
    double Liters,
    string Category,
    DateTime? RecordedAt
);
