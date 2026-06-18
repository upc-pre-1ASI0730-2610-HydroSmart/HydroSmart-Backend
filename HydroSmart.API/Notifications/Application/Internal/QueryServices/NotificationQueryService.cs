using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Notifications.Domain.Model.Queries;
using HydroSmart.API.Notifications.Domain.Repositories;
using HydroSmart.API.Notifications.Domain.Services;

namespace HydroSmart.API.Notifications.Application.Internal.QueryServices;

public class NotificationQueryService : INotificationQueryService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationQueryService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query)
    {
        return await _notificationRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Notification>> Handle(GetUnreadNotificationsByUserIdQuery query)
    {
        return await _notificationRepository.FindUnreadByUserIdAsync(query.UserId);
    }

    public async Task<Notification?> Handle(GetNotificationByIdQuery query)
    {
        return await _notificationRepository.FindByIdAsync(query.NotificationId);
    }

    public async Task<int> Handle(CountUnreadNotificationsQuery query)
    {
        return await _notificationRepository.CountUnreadByUserIdAsync(query.UserId);
    }
}

