namespace HydroSmart.API.Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    public int Id { get; }
    public int UserId { get; private set; }
    public string PhotoUrl { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Address { get; private set; }
    public string PhoneNumber { get; private set; }

    public Profile()
    {
        PhotoUrl = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Address = string.Empty;
        PhoneNumber = string.Empty;
    }

    public Profile(int userId, string photoUrl, string firstName, string lastName, string address, string email, string phoneNumber)
    {
        UserId = userId;
        PhotoUrl = photoUrl;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
    }


    public Profile UpdateInformation(string photoUrl, string firstName, string lastName, string address, string email, string phoneNumber)
    {
        PhotoUrl = photoUrl;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        return this;
    }
}