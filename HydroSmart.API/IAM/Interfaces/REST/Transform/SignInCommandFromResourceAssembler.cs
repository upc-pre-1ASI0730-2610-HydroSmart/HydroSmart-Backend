using HydroSmart.API.IAM.Domain.Model.Commands;
using HydroSmart.API.IAM.Interfaces.REST.Resources;

namespace HydroSmart.API.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}
