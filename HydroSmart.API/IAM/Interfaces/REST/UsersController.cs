using System.Net.Mime;
using HydroSmart.API.IAM.Domain.Model.Queries;
using HydroSmart.API.IAM.Domain.Services;
using HydroSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using HydroSmart.API.IAM.Interfaces.REST.Resources;
using HydroSmart.API.IAM.Interfaces.REST.Transform;
using HydroSmart.API.Profiles.Interfaces.ACL;
using HydroSmart.API.Profiles.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HydroSmart.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User Endpoints.")]
public class UsersController(
    IUserQueryService userQueryService,
    IUserCommandService userCommandService,
    IProfilesContextFacade profilesContextFacade) : ControllerBase
{
    [HttpGet("{userId:int}")]
    [SwaggerOperation("Get User by Id", "Get a user by its unique identifier.", OperationId = "GetUserById")]
    [SwaggerResponse(200, "The user was found and returned.", typeof(UserResource))]
    [SwaggerResponse(404, "The user was not found.")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        if (user is null) return NotFound();
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(userResource);
    }

    [HttpGet]
    [SwaggerOperation("Get All Users", "Get all users.", OperationId = "GetAllUsers")]
    [SwaggerResponse(200, "The users were found and returned.", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }

    [HttpGet("{userId:int}/profiles")]
    [SwaggerOperation("Get Profile by User Id", "Get the profile associated with a user by its unique identifier.", OperationId = "GetProfileByUserId")]
    [SwaggerResponse(200, "The profile was found and returned.", typeof(ProfileResource))]
    [SwaggerResponse(404, "The profile was not found.")]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        var profile = await profilesContextFacade.FetchProfileByUserId(userId);
        if (profile is null) return NotFound();
        return Ok(profile);
    }
    
    [HttpPut("{userId:int}/password")]
    [SwaggerOperation("Change User Password", "Change the password of a user.", OperationId = "ChangeUserPassword")]
    [SwaggerResponse(204, "The password was changed successfully.")]
    [SwaggerResponse(400, "Invalid password data.")]
    [SwaggerResponse(404, "The user was not found.")]
    public async Task<IActionResult> ChangePassword(int userId, [FromBody] UpdatePasswordResource resource)
    {
        if (resource.NewPassword != resource.ConfirmNewPassword)
            return BadRequest("NewPassword and ConfirmNewPassword do not match.");

        var command = UpdatePasswordCommandFromResourceAssembler.ToCommandFromResource(userId, resource);
        await userCommandService.Handle(command);
        return NoContent();
    }
}