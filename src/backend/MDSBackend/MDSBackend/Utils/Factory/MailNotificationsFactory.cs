using System.Net.Mail;

namespace MDSBackend.Utils.Factory;

public class MailNotificationsFactory
{
    public static Notification CreateNotification(NotificationInformationType type, 
        string title, 
        string message,List<Attachment> attachments)
    {
        return new MailNotification(type, title, message, attachments);
    }
    public Notification CreateNotification(NotificationInformationType type, 
        string title, 
        string message)
    {
        
        return new MailNotification(type, title, message);
        
    }
}