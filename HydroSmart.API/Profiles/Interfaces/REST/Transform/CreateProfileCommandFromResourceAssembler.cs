using HydroSmart.API.Profiles.Domain.Model.Commands;
using HydroSmart.API.Profiles.Interfaces.REST.Resources;

namespace HydroSmart.API.Profiles.Interfaces.REST.Transform;

public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(

            resource.UserId,
            resource.PhotoUrl,
            resource.FirstName,
            resource.LastName,
            resource.Address,
            resource.Email,
            resource.PhoneNumber

        );
    }
}