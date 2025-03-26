namespace MDSBackend.Utils.Factory;

public class PushNotificationsFactory
{
    public Notification CreateNotification(NotificationInformationType type,
        string title, 
        string message)
    {
        return new PushNotification(type, title, message);
    }
    
    public Notification CreateNotification(NotificationInformationType type, 
        string title, 
        string message,
        string image)
    {
        return new PushNotification(type, title, message, image);
    }
    public Notification CreateNotification(NotificationInformationType type, 
        string title, 
        string message,
        string? image,
        string clickAction,
        ClickActionType clickActionType)
    {
        return new PushNotification(type, title, message, image, clickAction, clickActionType);
    }
}