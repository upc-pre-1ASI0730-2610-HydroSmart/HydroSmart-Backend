using HydroSmart.API.Notifications.Domain.Model.Commands;
using HydroSmart.API.Notifications.Domain.Model.Queries;
using HydroSmart.API.Notifications.Domain.Services;
using HydroSmart.API.Notifications.Interfaces.ACL;
using HydroSmart.API.Notifications.Interfaces.REST.Resources;
using HydroSmart.API.Notifications.Interfaces.REST.Transform;

namespace HydroSmart.API.Notifications.Application.ACL;

public class NotificationsContextFacade : INotificationsContextFacade
{
    private readonly INotificationQueryService _notificationQueryService;
    private readonly INotificationCommandService _notificationCommandService;

    public NotificationsContextFacade(
        INotificationQueryService notificationQueryService,
        INotificationCommandService notificationCommandService)
    {
        _notificationQueryService = notificationQueryService;
        _notificationCommandService = notificationCommandService;
    }

    public async Task<IEnumerable<NotificationResource>> FetchNotificationsByUserId(int userId)
    {
        var query = new GetNotificationsByUserIdQuery(userId);
        var notifications = await _notificationQueryService.Handle(query);
        return NotificationResourceFromEntityAssembler.ToResourcesFromEntities(notifications);
    }

    public async Task<IEnumerable<NotificationResource>> FetchUnreadNotificationsByUserId(int userId)
    {
        var query = new GetUnreadNotificationsByUserIdQuery(userId);
        var notifications = await _notificationQueryService.Handle(query);
        return NotificationResourceFromEntityAssembler.ToResourcesFromEntities(notifications);
    }

    public async Task<NotificationResource?> FetchNotificationById(int notificationId)
    {
        var query = new GetNotificationByIdQuery(notificationId);
        var notification = await _notificationQueryService.Handle(query);
        return notification == null ? null : NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
    }

    public async Task<int> CountUnreadNotifications(int userId)
    {
        var query = new CountUnreadNotificationsQuery(userId);
        return await _notificationQueryService.Handle(query);
    }

    public async Task<NotificationResource?> CreateNotification(CreateNotificationRequest request)
    {
        var command = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(request);
        var notification = await _notificationCommandService.Handle(command);
        return notification == null ? null : NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
    }

    public async Task<NotificationResource?> MarkAsRead(int notificationId)
    {
        var command = new MarkNotificationAsReadCommand(notificationId);
        var notification = await _notificationCommandService.Handle(command);
        return notification == null ? null : NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
    }

    public async Task<NotificationResource?> MarkAsUnread(int notificationId)
    {
        var command = new MarkNotificationAsUnreadCommand(notificationId);
        var notification = await _notificationCommandService.Handle(command);
        return notification == null ? null : NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
    }

    public async Task<bool> DeleteNotification(int notificationId)
    {
        var command = new DeleteNotificationCommand(notificationId);
        return await _notificationCommandService.Handle(command);
    }
}

