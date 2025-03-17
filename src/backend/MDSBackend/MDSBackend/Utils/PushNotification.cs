namespace MDSBackend.Utils;

public class PushNotification : Notification
{
    #region Fields

    private readonly string? _image;
    private readonly string? _clickAction;
    private readonly ClickActionType? _clickActionType;

    #endregion

    #region Properties
    
    public string? Image { get => _image;  }
    public string? ClickAction { get => _clickAction;  }
    public int? ClickActionType { get => (int)_clickActionType;  }

    #endregion
    #region Constructor

    public PushNotification(NotificationInformationType type, string title, string message, string image, string clickAction, ClickActionType clickActionType) : base(type, title, message)
    {
        _image = image;
        _clickAction = clickAction;
        _clickActionType = clickActionType;
    }

    public PushNotification(NotificationInformationType type, string title, string message, string? image) : base(type, title, message)
    {
        _image = image;
    }

    public PushNotification(NotificationInformationType type, string title, string message) : base(type, title, message)
    {
    }

    #endregion
}