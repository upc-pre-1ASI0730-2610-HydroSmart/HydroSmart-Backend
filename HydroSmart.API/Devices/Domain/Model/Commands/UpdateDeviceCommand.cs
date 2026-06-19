namespace HydroSmart.API.Devices.Domain.Model.Commands;

public record UpdateDeviceCommand(
    int Id,
    string Name,
    string Section,
    string Status,
    string LastActive,
    int Alerts,
    decimal Consumption
    );