namespace HydroSmart.API.Profiles.Interfaces.REST.Resources;

public record ProfileResource(
    int Id,
    int UserId, 
    string PhotoUrl, 
    string FirstName,
    string LastName,
    string Address,
    string Email,
    string PhoneNumber
    
    );