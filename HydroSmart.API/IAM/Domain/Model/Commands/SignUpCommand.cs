namespace HydroSmart.API.IAM.Domain.Model.Commands;

/// <summary>
/// Sign up command
/// </summary>
/// <remarks>
/// This command object includes the email, password and role to sign up
/// </remarks>
public record SignUpCommand(string Email, string Password, string Role);
