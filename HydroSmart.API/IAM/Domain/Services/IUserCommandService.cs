using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Domain.Model.Commands;

namespace HydroSmart.API.IAM.Domain.Services;

/// <summary>
/// User command service interface
/// </summary>
/// <remarks>
/// This interface is used to handle user commands
/// </remarks>
public interface IUserCommandService
{
    /// <summary>
    /// Handle sign in command
    /// </summary>
    /// <param name="command">The sign in command</param>
    /// <returns>The authenticated user and the JWT token</returns>
    Task<(User user, string token)> Handle(SignInCommand command);

    /// <summary>
    /// Handle sign up command
    /// </summary>
    /// <param name="command">The sign up command</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task Handle(SignUpCommand command);
    
    /// <summary>
    /// Handle update password command
    /// </summary>
    /// <param name="command">The update password command</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task Handle(UpdatePasswordCommand command);
}
