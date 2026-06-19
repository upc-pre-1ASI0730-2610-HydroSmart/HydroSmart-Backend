using System.Text.Json.Serialization;

namespace HydroSmart.API.IAM.Domain.Model.Aggregates;

/// <summary>
/// User Aggregate Root
/// </summary>
/// <remarks>
/// This class represents the User aggregate root.
/// It contains the properties and methods to manage the user information.
/// </remarks>
public partial class User(string email, string passwordHash, string role)
{
    public User() : this(string.Empty, string.Empty, "User")
    {
    }

    public int Id { get; }
    public string Email { get; private set; } = email;
    
    [JsonIgnore]
    public string PasswordHash { get; private set; } = passwordHash;
    
    public string Role { get; private set; } = role;

    /// <summary>
    /// Update the email
    /// </summary>
    /// <param name="email">The new email</param>
    /// <returns>The updated user</returns>
    public User UpdateEmail(string email)
    {
        Email = email;
        return this;
    }

    /// <summary>
    /// Update the password hash
    /// </summary>
    /// <param name="passwordHash">The new password hash</param>
    /// <returns>The updated user</returns>
    public User UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }
    
    /// <summary>
    /// Update the role
    /// </summary>
    /// <param name="role">The new role</param>
    /// <returns>The updated user</returns>
    public User UpdateRole(string role)
    {
        Role = role;
        return this;
    }
}
