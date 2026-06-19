using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Domain.Model.Queries;

namespace HydroSmart.API.IAM.Domain.Services;

public interface IUserQueryService
{
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
    Task<User?> Handle(GetUserByIdQuery query);
}