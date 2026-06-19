using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Domain.Model.Queries;
using HydroSmart.API.Devices.Domain.Repositories;
using HydroSmart.API.Devices.Domain.Services;

namespace HydroSmart.API.Devices.Application.Internal.QueryServices;

public class DeviceQueryService(IDeviceRepository deviceRepository) : IDeviceQueryService
{
    public async Task<IEnumerable<Device>> Handle(GetAllDevicesQuery query)
    {
        return await deviceRepository.ListAsync();
    }

    public async Task<Device?> Handle(GetDeviceByIdQuery query)
    {
        return await deviceRepository.FindByIdAsync(query.DeviceId);
    }

    public async Task<IEnumerable<Device>> Handle(GetDevicesBySectionQuery query)
    {
        return await deviceRepository.FindBySectionAsync(query.Section);
    }
}