using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Devices.Infrastructure.Persistence.EFC.Repositories;

public class DeviceRepository(AppDbContext context) : BaseRepository<Device>(context), IDeviceRepository
{
    public async Task<IEnumerable<Device>> FindBySectionAsync(string section)
    {
        return await Context.Set<Device>()
            .Where(device => device.Section.ToLower().Contains(section.ToLower()))
            .ToListAsync();
    }
}