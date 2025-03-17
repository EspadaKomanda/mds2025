using System.Net.Mail;
using System.Runtime.InteropServices;

namespace MDSBackend.Utils;

public class NotificationsFactory
{
    #region FactoryMethods

    public static Notification CreateNotification(NotificationInformationType type, 
        NotificationType notificationType,
        string title, 
        string message,List<Attachment> attachments)
    {
        switch (notificationType)
        {
            case NotificationType.PUSH:
                return new PushNotification(type, title, message);
            case NotificationType.EMAIL:
                return new MailNotification(type, title, message, attachments);
            default:
                return new PushNotification(type, title, message);
        }
    }
    public Notification CreateNotification(NotificationInformationType type, 
        NotificationType notificationType,
        string title, 
        string message)
    {
        switch (notificationType)
        {
            case NotificationType.PUSH:
                return new PushNotification(type, title, message);
            case NotificationType.EMAIL:
                return new MailNotification(type, title, message);
            default:
                return new PushNotification(type, title, message);
        }
    }
    #endregion
}