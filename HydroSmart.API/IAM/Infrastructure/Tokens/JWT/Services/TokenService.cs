using System.Security.Claims;
using System.Text;
using HydroSmart.API.IAM.Application.Internal.OutboundServices;
using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HydroSmart.API.IAM.Infrastructure.Tokens.JWT.Services;

/// <summary>
/// Token service
/// </summary>
/// <remarks>
/// This class is used to generate and validate JWT tokens
/// </remarks>
public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;

    /// <summary>
    /// Generate token
    /// </summary>
    /// <param name="user">The user for token generation</param>
    /// <returns>The generated token</returns>
    public string GenerateToken(User user)
    {
        var secret = _tokenSettings.Secret;
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }

    /// <summary>
    /// Validate token
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <returns>The user id if the token is valid, null otherwise</returns>
    public async Task<int?> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
        try
        {
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
            var userId = int.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
            return userId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
