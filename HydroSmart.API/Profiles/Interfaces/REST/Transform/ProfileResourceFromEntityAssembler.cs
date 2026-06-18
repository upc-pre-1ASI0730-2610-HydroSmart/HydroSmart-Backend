using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Interfaces.REST.Resources;

namespace HydroSmart.API.Profiles.Interfaces.REST.Transform;

public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity)
    {
        return new ProfileResource(
            entity.Id,
            entity.UserId,
            entity.PhotoUrl,
            entity.FirstName,
            entity.LastName,
            entity.Address,
            entity.Email,
            entity.PhoneNumber
            );
    }
}