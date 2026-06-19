namespace HydroSmart.API.Devices.Interfaces.REST.Resources;

public record UpdateDeviceResource(
    string Name,
    string Section,
    string? Status,
    string? LastActive,
    int? Alerts,
    decimal? Consumption
);