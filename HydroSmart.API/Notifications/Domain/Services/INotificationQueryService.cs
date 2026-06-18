using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Notifications.Domain.Model.Queries;

namespace HydroSmart.API.Notifications.Domain.Services;

public interface INotificationQueryService
{
    Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query);
    Task<IEnumerable<Notification>> Handle(GetUnreadNotificationsByUserIdQuery query);
    Task<Notification?> Handle(GetNotificationByIdQuery query);
    Task<int> Handle(CountUnreadNotificationsQuery query);
}

