using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Domain.Model.Queries;

namespace HydroSmart.API.Profiles.Domain.Services;

public interface IProfileQueryService
{
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query);
    Task<Profile?> Handle(GetProfileByIdQuery query);
    Task<Profile?> Handle(GetProfileByUserIdQuery query);
}