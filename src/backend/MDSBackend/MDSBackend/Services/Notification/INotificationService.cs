using System.Net.Mail;
using MDSBackend.Models.Database;
using MDSBackend.Utils;

namespace MDSBackend.Services.NotificationService;

public interface INotificationService
{
    public Task SendMailNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType,NotificationType type);
    
}