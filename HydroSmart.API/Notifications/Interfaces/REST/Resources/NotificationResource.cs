namespace HydroSmart.API.Notifications.Interfaces.REST.Resources;

public class NotificationResource
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}

public class CreateNotificationRequest
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Type { get; set; } // INFO, WARNING, ERROR, SUCCESS
}

