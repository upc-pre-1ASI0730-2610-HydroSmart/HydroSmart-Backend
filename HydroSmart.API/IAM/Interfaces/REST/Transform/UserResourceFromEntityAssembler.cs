using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Interfaces.REST.Resources;

namespace HydroSmart.API.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity)
    {
        return new UserResource(entity.Id, entity.Email, entity.Role);
    }
}