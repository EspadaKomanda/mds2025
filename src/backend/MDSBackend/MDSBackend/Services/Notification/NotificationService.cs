using System.Net.Mail;
using MDSBackend.Models.Database;
using MDSBackend.Utils;

namespace MDSBackend.Services.NotificationService;

public class NotificationService : INotificationService
{
    #region Services

    private readonly EmailClient _emailClient;
    private readonly NotificationsFactory _notificationsFactory;

    #endregion

    #region Constructor

    public NotificationService(EmailClient emailClient, NotificationsFactory notificationsFactory)
    {
        _emailClient = emailClient;
        _notificationsFactory = notificationsFactory;
    }

    #endregion
    /// <summary>
    /// Sends the given notification to the given user.
    /// </summary>
    /// <param name="user">The user to send the notification to.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The message of the notification.</param>
    /// <param name="notificationInformationType">The information type of the notification.</param>
    /// <param name="type">The type of the notification.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType,NotificationType type)
    {
        
        switch(type)
        {
            case NotificationType.PUSH:
                break;
            case NotificationType.EMAIL:
                var notification = _notificationsFactory.CreateNotification(notificationInformationType, type, title, message);
                await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
                break;
            default:
                break;
        }
    }

    public async Task SendNotificationAsync(ApplicationUser user, string title, string message, NotificationInformationType notificationInformationType,NotificationType type, List<Attachment> attachments)
    {
        throw new NotImplementedException();
    }
}