using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Domain.Model.Commands;

namespace HydroSmart.API.Devices.Domain.Services;

public interface IDeviceCommandService
{
    Task<Device?> Handle(CreateDeviceCommand command);
    Task<Device?> Handle(UpdateDeviceCommand command);
    Task<bool> HandleDelete(int deviceId);
}