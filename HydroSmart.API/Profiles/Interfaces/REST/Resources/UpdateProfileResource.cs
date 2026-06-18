namespace HydroSmart.API.Profiles.Interfaces.REST.Resources;

public record UpdateProfileResource(
    int Id,
    string PhotoUrl, 
    string FirstName,
    string LastName,
    string Address,
    string Email,
    string PhoneNumber);