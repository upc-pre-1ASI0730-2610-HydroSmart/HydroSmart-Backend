using HydroSmart.API.Notifications.Interfaces.REST.Resources;

namespace HydroSmart.API.Notifications.Interfaces.ACL;

public interface INotificationsContextFacade
{
    Task<IEnumerable<NotificationResource>> FetchNotificationsByUserId(int userId);
    Task<IEnumerable<NotificationResource>> FetchUnreadNotificationsByUserId(int userId);
    Task<NotificationResource?> FetchNotificationById(int notificationId);
    Task<int> CountUnreadNotifications(int userId);
    Task<NotificationResource?> CreateNotification(CreateNotificationRequest request);
    Task<NotificationResource?> MarkAsRead(int notificationId);
    Task<NotificationResource?> MarkAsUnread(int notificationId);
    Task<bool> DeleteNotification(int notificationId);
}

