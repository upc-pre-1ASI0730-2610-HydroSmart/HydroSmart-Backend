using Cortex.Mediator.Notifications;
using HydroSmart.API.Shared.Domain.Model.Events;

namespace HydroSmart.API.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent, INotification
{
    
}