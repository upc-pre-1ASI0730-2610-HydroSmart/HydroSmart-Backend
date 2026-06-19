using HydroSmart.API.Devices.Domain.Model.Commands;
using HydroSmart.API.Devices.Interfaces.REST.Resources;

namespace HydroSmart.API.Devices.Interfaces.REST.Transform;

public static class CreateDeviceCommandFromResourceAssembler
{
    public static CreateDeviceCommand ToCommandFromResource(CreateDeviceResource resource)
    {
        return new CreateDeviceCommand(
            resource.Name,
            resource.Section,
            resource.Status ?? "inactive",
            resource.LastActive ?? "0 h",
            resource.Alerts ?? 0,
            resource.Consumption ?? 0
        );
    }
}