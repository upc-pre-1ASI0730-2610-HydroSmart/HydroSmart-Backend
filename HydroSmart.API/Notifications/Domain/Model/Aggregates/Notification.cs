namespace HydroSmart.API.Notifications.Domain.Model.Aggregates;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // INFO, WARNING, ERROR, SUCCESS
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }

    public Notification()
    {
    }

    public Notification(int userId, string title, string message, string type)
    {
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }

    public void MarkAsUnread()
    {
        IsRead = false;
        ReadAt = null;
    }
}

