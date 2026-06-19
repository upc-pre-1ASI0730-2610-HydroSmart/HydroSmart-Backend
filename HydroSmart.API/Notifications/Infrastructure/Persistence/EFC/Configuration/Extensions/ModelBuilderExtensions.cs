using HydroSmart.API.Notifications.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HydroSmart.API.Notifications.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyNotificationsConfiguration(this ModelBuilder builder)
    {
        // Apply Notification entity configuration
        builder.ApplyConfiguration(new NotificationConfiguration());
    }
}