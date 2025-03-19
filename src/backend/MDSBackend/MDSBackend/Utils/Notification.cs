using System.Net.Mail;

namespace MDSBackend.Utils;

public abstract class Notification
{
    #region Fields

    private string _title;
    private string _message;
    private NotificationInformationType _type;

    #endregion

    #region Parameters 
    
    public string Title { get => _title;}
    public string Message { get => _message; }
    public NotificationInformationType Type { get => _type; }

    #endregion

    #region Constructor

    public Notification(NotificationInformationType type, string title, string message)
    {
        _type = type;
        _title = title;
        _message = message;
    } 

    #endregion
}