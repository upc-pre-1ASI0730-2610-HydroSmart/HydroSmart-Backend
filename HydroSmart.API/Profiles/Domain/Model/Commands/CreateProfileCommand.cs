namespace HydroSmart.API.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(

    int UserId, 
    string PhotoUrl, 
    string FirstName,
    string LastName,
    string Address,
    string Email,
    string PhoneNumber
);

