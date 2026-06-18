using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using HydroSmart.API.Shared.Domain.Repositories;

namespace HydroSmart.API.Notifications.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IEnumerable<Notification>> FindByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> FindUnreadByUserIdAsync(int userId);
    Task<int> CountUnreadByUserIdAsync(int userId);
}

