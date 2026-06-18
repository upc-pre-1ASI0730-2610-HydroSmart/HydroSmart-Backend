using HydroSmart.API.Profiles.Domain.Model.Commands;
using HydroSmart.API.Profiles.Interfaces.REST.Resources;

namespace HydroSmart.API.Profiles.Interfaces.REST.Transform;

public static class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommandFromResource(UpdateProfileResource resource, int profileId)
    {
        return new UpdateProfileCommand(
            profileId,
            resource.PhotoUrl,
            resource.FirstName,
            resource.LastName,
            resource.Address,
            resource.Email,
            resource.PhoneNumber
        );
    }
}