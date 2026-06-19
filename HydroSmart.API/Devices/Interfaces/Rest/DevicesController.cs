using System.Net.Mime;
using HydroSmart.API.Devices.Domain.Model.Queries;
using HydroSmart.API.Devices.Domain.Services;
using HydroSmart.API.Devices.Interfaces.REST.Resources;
using HydroSmart.API.Devices.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HydroSmart.API.Devices.Interfaces.REST;

[ApiController]
[Route("devices")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Device Endpoints.")]
public class DevicesController(
    IDeviceCommandService deviceCommandService,
    IDeviceQueryService deviceQueryService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation("Get All Devices", "Get all registered devices.", OperationId = "GetAllDevices")]
    [SwaggerResponse(200, "The devices were found and returned.", typeof(IEnumerable<DeviceResource>))]
    public async Task<IActionResult> GetAllDevices([FromQuery] string? section)
    {
        var devices = string.IsNullOrWhiteSpace(section)
            ? await deviceQueryService.Handle(new GetAllDevicesQuery())
            : await deviceQueryService.Handle(new GetDevicesBySectionQuery(section));

        var resources = devices.Select(DeviceResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("{deviceId:int}")]
    [SwaggerOperation("Get Device by Id", "Get a device by its unique identifier.", OperationId = "GetDeviceById")]
    [SwaggerResponse(200, "The device was found and returned.", typeof(DeviceResource))]
    [SwaggerResponse(404, "The device was not found.")]
    public async Task<IActionResult> GetDeviceById(int deviceId)
    {
        var device = await deviceQueryService.Handle(new GetDeviceByIdQuery(deviceId));

        if (device is null)
            return NotFound();

        var resource = DeviceResourceFromEntityAssembler.ToResourceFromEntity(device);

        return Ok(resource);
    }

    [HttpPost]
    [SwaggerOperation("Create Device", "Create a new device.", OperationId = "CreateDevice")]
    [SwaggerResponse(201, "The device was created successfully.", typeof(DeviceResource))]
    [SwaggerResponse(400, "The device could not be created.")]
    public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceResource resource)
    {
        var command = CreateDeviceCommandFromResourceAssembler.ToCommandFromResource(resource);

        var device = await deviceCommandService.Handle(command);

        if (device is null)
            return BadRequest();

        var deviceResource = DeviceResourceFromEntityAssembler.ToResourceFromEntity(device);

        return CreatedAtAction(
            nameof(GetDeviceById),
            new { deviceId = device.Id },
            deviceResource
        );
    }

    [HttpPut("{deviceId:int}")]
    [SwaggerOperation("Update Device", "Update an existing device by its unique identifier.", OperationId = "UpdateDevice")]
    [SwaggerResponse(200, "The device was updated successfully.", typeof(DeviceResource))]
    [SwaggerResponse(404, "The device was not found.")]
    public async Task<IActionResult> UpdateDevice(int deviceId, [FromBody] UpdateDeviceResource resource)
    {
        var currentDevice = await deviceQueryService.Handle(new GetDeviceByIdQuery(deviceId));

        if (currentDevice is null)
            return NotFound();

        var command = UpdateDeviceCommandFromResourceAssembler.ToCommandFromResource(
            resource,
            deviceId,
            currentDevice
        );

        var device = await deviceCommandService.Handle(command);

        if (device is null)
            return NotFound();

        var deviceResource = DeviceResourceFromEntityAssembler.ToResourceFromEntity(device);

        return Ok(deviceResource);
    }

    [HttpDelete("{deviceId:int}")]
    [SwaggerOperation("Delete Device", "Delete an existing device by its unique identifier.", OperationId = "DeleteDevice")]
    [SwaggerResponse(204, "The device was deleted successfully.")]
    [SwaggerResponse(404, "The device was not found.")]
    public async Task<IActionResult> DeleteDevice(int deviceId)
    {
        var deleted = await deviceCommandService.HandleDelete(deviceId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}