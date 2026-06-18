using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Domain.Model.Commands;

namespace HydroSmart.API.Profiles.Domain.Services;

public interface IProfileCommandService
{
    /// <summary>
    /// Handle Create Profile Command
    /// </summary>
    /// <param name="command">The <see cref="CreateProfileCommand"/> command</param>
    /// <returns>The created <see cref="Profile"/> object</returns>
    Task<Profile?> Handle(CreateProfileCommand command);

    /// <summary>
    /// Handle Update Profile Command
    /// </summary>
    /// <param name="command">The <see cref="UpdateProfileCommand"/> command</param>
    /// <returns>The updated <see cref="Profile"/> object</returns>
    Task<Profile?> Handle(UpdateProfileCommand command);
}