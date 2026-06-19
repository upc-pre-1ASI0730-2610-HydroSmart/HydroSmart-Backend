namespace HydroSmart.API.Devices.Domain.Model.Commands;

public record CreateDeviceCommand(
    string Name,
    string Section,
    string Status,
    string LastActive,
    int Alerts,
    decimal Consumption
    );