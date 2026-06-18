namespace HydroSmart.API.Notifications.Domain.Model.Commands;

public record CreateNotificationCommand(
    int UserId,
    string Title,
    string Message,
    string Type
);

public record MarkNotificationAsReadCommand(
    int NotificationId
);

public record MarkNotificationAsUnreadCommand(
    int NotificationId
);

public record DeleteNotificationCommand(
    int NotificationId
);

