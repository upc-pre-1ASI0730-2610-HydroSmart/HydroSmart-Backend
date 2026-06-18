using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Profiles.Domain.Repositories;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<Profile?> FindByUserIdAsync(int userId);
    Task<IEnumerable<Profile>> FindByUsernameAsync(string firstName, string lastName);
}