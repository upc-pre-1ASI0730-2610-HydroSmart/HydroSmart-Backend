using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Notifications.Interfaces.REST.Resources;

namespace HydroSmart.API.Notifications.Interfaces.REST.Transform;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification notification)
    {
        return new NotificationResource
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt
        };
    }

    public static IEnumerable<NotificationResource> ToResourcesFromEntities(IEnumerable<Notification> notifications)
    {
        return notifications.Select(ToResourceFromEntity);
    }
}

public static class CreateNotificationCommandFromResourceAssembler
{
    public static HydroSmart.API.Notifications.Domain.Model.Commands.CreateNotificationCommand ToCommandFromResource(CreateNotificationRequest resource)
    {
        return new HydroSmart.API.Notifications.Domain.Model.Commands.CreateNotificationCommand(
            resource.UserId,
            resource.Title,
            resource.Message,
            resource.Type
        );
    }
}

