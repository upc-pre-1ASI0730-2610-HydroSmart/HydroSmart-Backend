namespace HydroSmart.API.IAM.Interfaces.REST.Resources;

public class UpdatePasswordResource
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}