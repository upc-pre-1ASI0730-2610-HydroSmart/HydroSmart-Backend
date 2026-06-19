using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Find a user by email
    /// </summary>
    /// <param name="email">The email to search</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<User?> FindByEmailAsync(string email);

    /// <summary>
    /// Check if a user exists by email
    /// </summary>
    /// <param name="email">The email to search</param>
    /// <returns>True if the user exists, false otherwise</returns>
    bool ExistsByEmail(string email);
}