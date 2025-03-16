using MDSBackend.Models.Database;
using MDSBackend.Utils;

namespace MDSBackend.Services.NotificationService;

public interface INotificationService
{
    public Task SendNotificationAsync(ApplicationUser user, string message, NotificationType type);
}