namespace HydroSmart.API.Devices.Interfaces.REST.Resources;

public record DeviceResource(
    int Id,
    string Name,
    string Section,
    string Status,
    string LastActive,
    int Alerts,
    decimal Consumption
);