using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Domain.Model.Queries;

namespace HydroSmart.API.Devices.Domain.Services;

public interface IDeviceQueryService
{
    Task<IEnumerable<Device>> Handle(GetAllDevicesQuery query);
    Task<Device?> Handle(GetDeviceByIdQuery query);
    Task<IEnumerable<Device>> Handle(GetDevicesBySectionQuery query);
}