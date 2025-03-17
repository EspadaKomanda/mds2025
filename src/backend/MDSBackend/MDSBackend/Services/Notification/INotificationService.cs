using System.Net.Mail;
using MDSBackend.Models.Database;
using MDSBackend.Utils;

namespace MDSBackend.Services.NotificationService;

public interface INotificationService
{
    public Task SendNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType,NotificationType type);
    public Task SendNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType, NotificationType type, List<Attachment> attachments);
    public Task SendNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType,NotificationType type, string image);
    public Task SendNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType,NotificationType type, string topic, string? image);
}