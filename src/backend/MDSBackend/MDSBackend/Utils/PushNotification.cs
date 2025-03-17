namespace MDSBackend.Utils;

public class PushNotification : Notification
{
    public PushNotification(NotificationInformationType type, string title, string message) : base(type, title, message)
    {
    }
}