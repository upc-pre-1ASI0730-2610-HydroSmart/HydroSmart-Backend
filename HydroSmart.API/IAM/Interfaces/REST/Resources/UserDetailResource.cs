namespace HydroSmart.API.IAM.Interfaces.REST.Resources;

public record UserDetailResource(
    int Id, 
    int UserId, 
    string PhotoUrl, 
    string FirstName, 
    string LastName, 
    string Address, 
    string Email, 
    string PhoneNumber);