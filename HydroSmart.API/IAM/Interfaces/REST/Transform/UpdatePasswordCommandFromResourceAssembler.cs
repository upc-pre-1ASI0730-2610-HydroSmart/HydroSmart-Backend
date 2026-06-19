using HydroSmart.API.IAM.Domain.Model.Commands;
using HydroSmart.API.IAM.Interfaces.REST.Resources;

namespace HydroSmart.API.IAM.Interfaces.REST.Transform;

public class UpdatePasswordCommandFromResourceAssembler
{
    public static UpdatePasswordCommand ToCommandFromResource(int userId, UpdatePasswordResource resource)
        => new(userId, resource.CurrentPassword, resource.NewPassword);
}