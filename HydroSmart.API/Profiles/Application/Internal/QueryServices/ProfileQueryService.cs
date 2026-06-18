using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Domain.Model.Queries;
using HydroSmart.API.Profiles.Domain.Repositories;
using HydroSmart.API.Profiles.Domain.Services;

namespace HydroSmart.API.Profiles.Application.Internal.QueryServices;

public class ProfileQueryService : IProfileQueryService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileQueryService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
    {
        return await _profileRepository.ListAsync();
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query)
    {
        return await _profileRepository.FindByIdAsync(query.ProfileId);
    }

    public async Task<Profile?> Handle(GetProfileByUserIdQuery query)
    {
        return await _profileRepository.FindByUserIdAsync(query.UserId);
    }
}