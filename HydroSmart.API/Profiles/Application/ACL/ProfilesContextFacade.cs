using HydroSmart.API.Profiles.Domain.Model.Queries;
using HydroSmart.API.Profiles.Domain.Repositories;
using HydroSmart.API.Profiles.Domain.Services;
using HydroSmart.API.Profiles.Interfaces.ACL;
using HydroSmart.API.Profiles.Interfaces.REST.Resources;
using HydroSmart.API.Profiles.Interfaces.REST.Transform;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Profiles.Application.ACL;

public class ProfilesContextFacade : IProfilesContextFacade
{
    private readonly IProfileQueryService _profileQueryService;
    private readonly IProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProfilesContextFacade(
        IProfileQueryService profileQueryService,
        IProfileRepository profileRepository,
        IUnitOfWork unitOfWork)
    {
        _profileQueryService = profileQueryService;
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ProfileResource?> FetchProfileByUserId(int userId)
    {
        var getProfileByUserIdQuery = new GetProfileByUserIdQuery(userId);
        var profile = await _profileQueryService.Handle(getProfileByUserIdQuery);
        return profile == null ? null : ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
    }
}