using HydroSmart.API.IAM.Application.Internal.OutboundServices;
using BCryptNet = BCrypt.Net.BCrypt;

namespace HydroSmart.API.IAM.Infrastructure.Hashing.BCrypt.Services;

/// <summary>
/// Hashing service
/// </summary>
/// <remarks>
/// This class is responsible for hashing and validating passwords using BCrypt
/// </remarks>
public class HashingService : IHashingService
{
    /// <summary>
    /// Hash a password
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>The hashed password</returns>
    public string HashPassword(string password)
    {
        return BCryptNet.HashPassword(password);
    }

    /// <summary>
    /// Verify a password against a hash
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <param name="passwordHash">The hash to validate against</param>
    /// <returns>True if the password is valid, false otherwise</returns>
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCryptNet.Verify(password, passwordHash);
    }
}
