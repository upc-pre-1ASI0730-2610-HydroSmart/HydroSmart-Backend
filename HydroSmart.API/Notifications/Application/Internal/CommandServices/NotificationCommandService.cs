using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Notifications.Domain.Model.Commands;
using HydroSmart.API.Notifications.Domain.Repositories;
using HydroSmart.API.Notifications.Domain.Services;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Notifications.Application.Internal.CommandServices;

public class NotificationCommandService : INotificationCommandService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly HydroSmart.API.Shared.Domain.Repositories.IUnitOfWork _unitOfWork;

    public NotificationCommandService(
        INotificationRepository notificationRepository,
        HydroSmart.API.Shared.Domain.Repositories.IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Notification?> Handle(CreateNotificationCommand command)
    {
        var notification = new Notification(
            command.UserId,
            command.Title,
            command.Message,
            command.Type
        );

        try
        {
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating notification: {ex.Message}");
        }

        return notification;
    }

    public async Task<Notification?> Handle(MarkNotificationAsReadCommand command)
    {
        var notification = await _notificationRepository.FindByIdAsync(command.NotificationId);
        if (notification == null)
            throw new Exception($"Notification with Id {command.NotificationId} not found.");

        notification.MarkAsRead();

        try
        {
            _notificationRepository.Update(notification);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error marking notification as read: {ex.Message}");
        }

        return notification;
    }

    public async Task<Notification?> Handle(MarkNotificationAsUnreadCommand command)
    {
        var notification = await _notificationRepository.FindByIdAsync(command.NotificationId);
        if (notification == null)
            throw new Exception($"Notification with Id {command.NotificationId} not found.");

        notification.MarkAsUnread();

        try
        {
            _notificationRepository.Update(notification);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error marking notification as unread: {ex.Message}");
        }

        return notification;
    }

    public async Task<bool> Handle(DeleteNotificationCommand command)
    {
        var notification = await _notificationRepository.FindByIdAsync(command.NotificationId);
        if (notification == null)
            throw new Exception($"Notification with Id {command.NotificationId} not found.");

        try
        {
            _notificationRepository.Remove(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting notification: {ex.Message}");
        }
    }
}

