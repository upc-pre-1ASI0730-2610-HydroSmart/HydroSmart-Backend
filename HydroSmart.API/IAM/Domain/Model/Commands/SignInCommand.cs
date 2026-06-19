namespace HydroSmart.API.IAM.Domain.Model.Commands;

/// <summary>
/// Sign in command
/// </summary>
/// <remarks>
/// This command object includes the email and password to sign in
/// </remarks>
public record SignInCommand(string Email, string Password);
