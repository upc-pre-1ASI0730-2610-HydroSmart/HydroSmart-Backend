using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    /// <summary>
    /// Find a user by email
    /// </summary>
    /// <param name="email">The email to search</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// Check if a user exists by email
    /// </summary>
    /// <param name="email">The email to search</param>
    /// <returns>True if the user exists, false otherwise</returns>
    public bool ExistsByEmail(string email)
    {
        return Context.Set<User>().Any(u => u.Email == email);
    }
}