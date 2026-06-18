namespace HydroSmart.API.Notifications.Domain.Model.Queries;

public record GetNotificationsByUserIdQuery(int UserId);

public record GetUnreadNotificationsByUserIdQuery(int UserId);

public record GetNotificationByIdQuery(int NotificationId);

public record CountUnreadNotificationsQuery(int UserId);

