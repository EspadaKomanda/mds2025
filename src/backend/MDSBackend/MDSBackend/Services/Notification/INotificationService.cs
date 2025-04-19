using System.Net.Mail;
using MDSBackend.Models.Database;
using MDSBackend.Utils;

namespace MDSBackend.Services.NotificationService;

public interface INotificationService
{
    public Task SendMailNotificationAsync(ApplicationUser user, Notification notification);
    public Task SendPushNotificationAsync(ApplicationUser user, Notification notification);
}