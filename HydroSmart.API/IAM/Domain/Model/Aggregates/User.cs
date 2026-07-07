using System.Text.Json.Serialization;

namespace HydroSmart.API.IAM.Domain.Model.Aggregates;

public partial class User(string email, string passwordHash, string role)
{
    public User() : this(string.Empty, string.Empty, "User")
    {
    }

    public int Id { get; private set; }

    public string Email { get; private set; } = email;

    [JsonIgnore]
    public string PasswordHash { get; private set; } = passwordHash;

    public string Role { get; private set; } = role;

    public User UpdateEmail(string email)
    {
        Email = email;
        return this;
    }

    public User UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }

    public User UpdateRole(string role)
    {
        Role = role;
        return this;
    }
}