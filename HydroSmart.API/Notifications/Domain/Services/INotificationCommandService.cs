using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Notifications.Domain.Model.Commands;

namespace HydroSmart.API.Notifications.Domain.Services;

public interface INotificationCommandService
{
    Task<Notification?> Handle(CreateNotificationCommand command);
    Task<Notification?> Handle(MarkNotificationAsReadCommand command);
    Task<Notification?> Handle(MarkNotificationAsUnreadCommand command);
    Task<bool> Handle(DeleteNotificationCommand command);
}

