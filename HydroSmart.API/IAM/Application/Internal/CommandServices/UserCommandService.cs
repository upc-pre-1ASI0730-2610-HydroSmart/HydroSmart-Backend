using HydroSmart.API.IAM.Application.Internal.OutboundServices;
using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Domain.Model.Commands;
using HydroSmart.API.IAM.Domain.Repositories;
using HydroSmart.API.IAM.Domain.Services;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.IAM.Application.Internal.CommandServices;

/// <summary>
/// User command service
/// </summary>
/// <remarks>
/// This class is used to handle user commands for authentication
/// </remarks>
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /// <summary>
    /// Handle sign in command
    /// </summary>
    /// <param name="command">The sign in command</param>
    /// <returns>The authenticated user and the JWT token</returns>
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByEmailAsync(command.Email);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid email or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    /// <summary>
    /// Handle sign up command
    /// </summary>
    /// <param name="command">The sign-up command</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByEmail(command.Email))
            throw new Exception($"Email {command.Email} is already taken");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Email, hashedPassword, command.Role);
        
        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
    
    /// <summary>
    /// Handle update password command
    /// </summary>
    /// <param name="command">The update password command</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task Handle(UpdatePasswordCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null)
            throw new Exception("User not found");

        if (!hashingService.VerifyPassword(command.CurrentPassword, user.PasswordHash))
            throw new Exception("Invalid current password");

        if (command.NewPassword.Length < 8)
            throw new Exception("Password must be at least 8 characters long");

        var newPasswordHash = hashingService.HashPassword(command.NewPassword);
        user.UpdatePasswordHash(newPasswordHash);

        userRepository.Update(user);
        await unitOfWork.CompleteAsync();
    }
}
