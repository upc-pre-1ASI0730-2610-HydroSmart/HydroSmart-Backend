namespace HydroSmart.API.Profiles.Domain.Model.Commands;

public record UpdateProfileCommand(
    int Id,
    string PhotoUrl,
    string FirstName,
    string LastName,
    string Address,
    string Email,
    string PhoneNumber
);
