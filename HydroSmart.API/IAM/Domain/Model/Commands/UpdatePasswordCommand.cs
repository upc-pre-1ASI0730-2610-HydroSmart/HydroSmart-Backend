namespace HydroSmart.API.IAM.Domain.Model.Commands;

public record UpdatePasswordCommand(int UserId, string CurrentPassword, string NewPassword);