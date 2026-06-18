using HydroSmart.API.Profiles.Interfaces.REST.Resources;

namespace HydroSmart.API.Profiles.Interfaces.ACL;

public interface IProfilesContextFacade
{
    /// <summary>
    /// Fetch profile by user id
    /// </summary>
    /// <param name="userId">
    /// User id of the profile to fetch
    /// </param>
    /// <returns>
    /// The profile resource if found, null otherwise
    /// </returns>
    Task<ProfileResource?> FetchProfileByUserId(int userId);
    
}