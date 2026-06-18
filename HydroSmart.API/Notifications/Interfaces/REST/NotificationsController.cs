using HydroSmart.API.Notifications.Interfaces.ACL;
using HydroSmart.API.Notifications.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace HydroSmart.API.Notifications.Interfaces.REST;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationsContextFacade _notificationsContextFacade;

    public NotificationsController(INotificationsContextFacade notificationsContextFacade)
    {
        _notificationsContextFacade = notificationsContextFacade;
    }

    /// <summary>
    /// Get all notifications for a specific user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<NotificationResource>>> GetNotificationsByUserId(int userId)
    {
        var notifications = await _notificationsContextFacade.FetchNotificationsByUserId(userId);
        return Ok(notifications);
    }

    /// <summary>
    /// Get unread notifications for a specific user
    /// </summary>
    [HttpGet("user/{userId}/unread")]
    public async Task<ActionResult<IEnumerable<NotificationResource>>> GetUnreadNotificationsByUserId(int userId)
    {
        var notifications = await _notificationsContextFacade.FetchUnreadNotificationsByUserId(userId);
        return Ok(notifications);
    }

    /// <summary>
    /// Get notification count unread for a specific user
    /// </summary>
    [HttpGet("user/{userId}/unread-count")]
    public async Task<ActionResult<int>> CountUnreadNotifications(int userId)
    {
        var count = await _notificationsContextFacade.CountUnreadNotifications(userId);
        return Ok(count);
    }

    /// <summary>
    /// Get a specific notification by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<NotificationResource>> GetNotificationById(int id)
    {
        var notification = await _notificationsContextFacade.FetchNotificationById(id);
        if (notification == null)
            return NotFound();
        return Ok(notification);
    }

    /// <summary>
    /// Create a new notification
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<NotificationResource>> CreateNotification([FromBody] CreateNotificationRequest request)
    {
        var notification = await _notificationsContextFacade.CreateNotification(request);
        return CreatedAtAction(nameof(GetNotificationById), new { id = notification?.Id }, notification);
    }

    /// <summary>
    /// Mark a notification as read
    /// </summary>
    [HttpPut("{id}/mark-as-read")]
    public async Task<ActionResult<NotificationResource>> MarkAsRead(int id)
    {
        var notification = await _notificationsContextFacade.MarkAsRead(id);
        if (notification == null)
            return NotFound();
        return Ok(notification);
    }

    /// <summary>
    /// Mark a notification as unread
    /// </summary>
    [HttpPut("{id}/mark-as-unread")]
    public async Task<ActionResult<NotificationResource>> MarkAsUnread(int id)
    {
        var notification = await _notificationsContextFacade.MarkAsUnread(id);
        if (notification == null)
            return NotFound();
        return Ok(notification);
    }

    /// <summary>
    /// Delete a notification
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var success = await _notificationsContextFacade.DeleteNotification(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}

