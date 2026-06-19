using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Interfaces.REST.Resources;

namespace HydroSmart.API.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Email, user.Role, token);
    }
}
