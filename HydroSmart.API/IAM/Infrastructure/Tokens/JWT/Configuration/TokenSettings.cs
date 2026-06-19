namespace HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Configuration;

/// <summary>
/// This class is used to store the token settings.
/// It is used to configure the token settings in the appsettings.json file.
/// </summary>
public class TokenSettings
{
    public string Secret { get; set; } = string.Empty;
}
