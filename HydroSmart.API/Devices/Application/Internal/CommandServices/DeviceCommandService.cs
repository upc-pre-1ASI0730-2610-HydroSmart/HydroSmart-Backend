using HydroSmart.API.Devices.Domain.Model.Aggregates;
using HydroSmart.API.Devices.Domain.Model.Commands;
using HydroSmart.API.Devices.Domain.Repositories;
using HydroSmart.API.Devices.Domain.Services;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Devices.Application.Internal.CommandServices;

public class DeviceCommandService(
    IDeviceRepository deviceRepository,
    IUnitOfWork unitOfWork) : IDeviceCommandService
{
    public async Task<Device?> Handle(CreateDeviceCommand command)
    {
        var device = new Device(
            command.Name,
            command.Section,
            string.IsNullOrWhiteSpace(command.Status) ? "inactive" : command.Status,
            string.IsNullOrWhiteSpace(command.LastActive) ? "0 h" : command.LastActive,
            command.Alerts,
            command.Consumption
        );

        await deviceRepository.AddAsync(device);
        await unitOfWork.CompleteAsync();

        return device;
    }

    public async Task<Device?> Handle(UpdateDeviceCommand command)
    {
        var device = await deviceRepository.FindByIdAsync(command.Id);

        if (device is null)
            return null;

        device.UpdateInformation(
            command.Name,
            command.Section,
            string.IsNullOrWhiteSpace(command.Status) ? device.Status : command.Status,
            string.IsNullOrWhiteSpace(command.LastActive) ? device.LastActive : command.LastActive,
            command.Alerts,
            command.Consumption
        );

        deviceRepository.Update(device);
        await unitOfWork.CompleteAsync();

        return device;
    }

    public async Task<bool> HandleDelete(int deviceId)
    {
        var device = await deviceRepository.FindByIdAsync(deviceId);

        if (device is null)
            return false;

        deviceRepository.Remove(device);
        await unitOfWork.CompleteAsync();

        return true;
    }
}