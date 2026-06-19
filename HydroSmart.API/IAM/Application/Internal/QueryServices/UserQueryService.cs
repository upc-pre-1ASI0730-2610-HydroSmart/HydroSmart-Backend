using HydroSmart.API.IAM.Domain.Model.Aggregates;
using HydroSmart.API.IAM.Domain.Model.Queries;
using HydroSmart.API.IAM.Domain.Repositories;
using HydroSmart.API.IAM.Domain.Services;

namespace HydroSmart.API.IAM.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await userRepository.ListAsync();
    }

    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.UserId);
    }
}