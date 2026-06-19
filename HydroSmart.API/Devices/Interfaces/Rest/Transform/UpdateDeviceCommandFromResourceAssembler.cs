using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Domain.Model.Commands;
using HydroSmart.API.Devices.Interfaces.REST.Resources;

namespace HydroSmart.API.Devices.Interfaces.REST.Transform;

public static class UpdateDeviceCommandFromResourceAssembler
{
    public static UpdateDeviceCommand ToCommandFromResource(
        UpdateDeviceResource resource,
        int deviceId,
        Device currentDevice)
    {
        return new UpdateDeviceCommand(
            deviceId,
            resource.Name,
            resource.Section,
            resource.Status ?? currentDevice.Status,
            resource.LastActive ?? currentDevice.LastActive,
            resource.Alerts ?? currentDevice.Alerts,
            resource.Consumption ?? currentDevice.Consumption
        );
    }
}