using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Domain.Repositories;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using HydroSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Profiles.Infrastructure.Persistence.EFC.Repositories;

public class ProfileRepository(AppDbContext context) : BaseRepository<Profile>(context), IProfileRepository
{
    public async Task<Profile?> FindByUserIdAsync(int userId)
    {
        return await Context.Set<Profile>().FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public Task<IEnumerable<Profile>> FindByUsernameAsync(string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Profile>> FindByFirstNameAsync(string firstName)
    {
        return await Context.Set<Profile>().Where(p => p.FirstName.Contains(firstName)).ToListAsync();
    }
}