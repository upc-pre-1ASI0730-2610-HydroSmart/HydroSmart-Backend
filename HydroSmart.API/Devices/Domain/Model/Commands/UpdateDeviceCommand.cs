namespace HydroSmart.API.Profiles.Domain.Model.Commands;

public record UpdateDeviceCommand(
    int Id,
    string Name,
    string Section,
    string Status,
    string LastActive,
    int Alerts,
    decimal Consumption
    );