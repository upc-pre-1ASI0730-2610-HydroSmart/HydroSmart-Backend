using System.Net.Mime;
using HydroSmart.API.Settings.Domain.Model.Queries;
using HydroSmart.API.Settings.Domain.Services;
using HydroSmart.API.Settings.Interfaces.REST.Resources;
using HydroSmart.API.Settings.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HydroSmart.API.Settings.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Settings Endpoints.")]
public class SettingsController(
    ISettingsCommandService settingsCommandService,
    ISettingsQueryService settingsQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation("Create Settings", "Create settings for a user.", OperationId = "CreateSettings")]
    [SwaggerResponse(201, "The settings were created successfully.", typeof(SettingsResource))]
    [SwaggerResponse(400, "The settings could not be created.")]
    public async Task<IActionResult> CreateSettings([FromBody] CreateSettingsResource resource)
    {
        var createSettingsCommand = CreateSettingsCommandFromResourceAssembler.ToCommandFromResource(resource);
        var settings = await settingsCommandService.Handle(createSettingsCommand);
        if (settings is null)
        {
            return Conflict();
        }

        var settingsResource = SettingsResourceFromEntityAssembler.ToResourceFromEntity(settings);
        return CreatedAtAction(nameof(GetSettingsById), new { settingsId = settings.Id }, settingsResource);
    }

    [HttpGet("{settingsId:int}")]
    [SwaggerOperation("Get Settings by Id", "Get settings by its unique identifier.", OperationId = "GetSettingsById")]
    [SwaggerResponse(200, "The settings were found and returned.", typeof(SettingsResource))]
    [SwaggerResponse(404, "The settings were not found.")]
    public async Task<IActionResult> GetSettingsById(int settingsId)
    {
        var getSettingsByIdQuery = new GetSettingsByIdQuery(settingsId);
        var settings = await settingsQueryService.Handle(getSettingsByIdQuery);
        if (settings is null)
        {
            return NotFound();
        }

        var settingsResource = SettingsResourceFromEntityAssembler.ToResourceFromEntity(settings);
        return Ok(settingsResource);
    }

    [HttpGet("user/{userId:int}")]
    [SwaggerOperation("Get Settings by User Id", "Get a user's settings.", OperationId = "GetSettingsByUserId")]
    [SwaggerResponse(200, "The settings were found and returned.", typeof(SettingsResource))]
    [SwaggerResponse(404, "The settings were not found.")]
    public async Task<IActionResult> GetSettingsByUserId(int userId)
    {
        var getSettingsByUserIdQuery = new GetSettingsByUserIdQuery(userId);
        var settings = await settingsQueryService.Handle(getSettingsByUserIdQuery);
        if (settings is null)
        {
            return NotFound();
        }

        var settingsResource = SettingsResourceFromEntityAssembler.ToResourceFromEntity(settings);
        return Ok(settingsResource);
    }

    [HttpGet]
    [SwaggerOperation("Get All Settings", "Get all settings.", OperationId = "GetAllSettings")]
    [SwaggerResponse(200, "The settings were found and returned.", typeof(IEnumerable<SettingsResource>))]
    public async Task<IActionResult> GetAllSettings()
    {
        var getAllSettingsQuery = new GetAllSettingsQuery();
        var settings = await settingsQueryService.Handle(getAllSettingsQuery);
        var settingsResources = settings.Select(SettingsResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(settingsResources);
    }

    [HttpPut("{settingsId:int}")]
    [SwaggerOperation("Update Settings", "Update settings by its unique identifier.", OperationId = "UpdateSettings")]
    [SwaggerResponse(200, "The settings were updated successfully.", typeof(SettingsResource))]
    [SwaggerResponse(404, "The settings were not found.")]
    [SwaggerResponse(400, "The settings could not be updated.")]
    public async Task<IActionResult> UpdateSettings(int settingsId, [FromBody] UpdateSettingsResource resource)
    {
        var updateSettingsCommand = UpdateSettingsCommandFromResourceAssembler.ToCommandFromResource(resource, settingsId);
        var settings = await settingsCommandService.Handle(updateSettingsCommand);
        if (settings is null)
        {
            return NotFound();
        }

        var settingsResource = SettingsResourceFromEntityAssembler.ToResourceFromEntity(settings);
        return Ok(settingsResource);
    }
}