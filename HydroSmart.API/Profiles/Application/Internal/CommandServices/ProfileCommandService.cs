using HydroSmart.API.Profiles.Domain.Model.Aggregates;
using HydroSmart.API.Profiles.Domain.Model.Commands;
using HydroSmart.API.Profiles.Domain.Repositories;
using HydroSmart.API.Profiles.Domain.Services;

namespace HydroSmart.API.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService : IProfileCommandService
{
    private readonly IProfileRepository _profileRepository;
    private readonly HydroSmart.API.Shared.Domain.Repositories.IUnitOfWork _unitOfWork;

    public ProfileCommandService(IProfileRepository profileRepository, HydroSmart.API.Shared.Domain.Repositories.IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Profile?> Handle(CreateProfileCommand command)
    {
        // Check if a profile with the same UserId already exists
        var existingProfile = await _profileRepository.FindByUserIdAsync(command.UserId);
        if (existingProfile != null)
        {
            throw new Exception($"A profile for UserId {command.UserId} already exists.");
        }

        var profile = new Profile(
            command.UserId,
            command.PhotoUrl,
            command.FirstName,
            command.LastName,
            command.Email,
            command.Address,
            command.PhoneNumber
        );

        try
        {
            await _profileRepository.AddAsync(profile);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating the profile: {e.Message}");
        }

        return profile;
    }

    /// <inheritdoc />
    public async Task<Profile?> Handle(UpdateProfileCommand command)
    {
        // Check if the profile exists
        var profile = await _profileRepository.FindByIdAsync(command.Id);
        if (profile == null)
        {
            throw new Exception($"Profile with Id {command.Id} not found.");
        }

        // Update the profile information
        profile.UpdateInformation(
            command.PhotoUrl,
            command.FirstName,
            command.LastName,
            command.Email,
            command.Address,
            command.PhoneNumber
        );

        try
        {
            _profileRepository.Update(profile);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while updating the profile: {e.Message}");
        }

        return profile;
    }
}