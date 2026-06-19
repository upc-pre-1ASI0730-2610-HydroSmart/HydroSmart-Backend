using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Devices.Domain.Repositories;

public interface IDeviceRepository : IBaseRepository<Device>
{
    Task<IEnumerable<Device>> FindBySectionAsync(string section);
}