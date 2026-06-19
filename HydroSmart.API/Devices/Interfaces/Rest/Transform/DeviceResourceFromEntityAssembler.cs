using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Interfaces.REST.Resources;

namespace HydroSmart.API.Devices.Interfaces.REST.Transform;

public static class DeviceResourceFromEntityAssembler
{
    public static DeviceResource ToResourceFromEntity(Device entity)
    {
        return new DeviceResource(
            entity.Id,
            entity.Name,
            entity.Section,
            entity.Status,
            entity.LastActive,
            entity.Alerts,
            entity.Consumption
        );
    }
}