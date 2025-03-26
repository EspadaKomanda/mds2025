using System.Net.Mail;

namespace MDSBackend.Utils;

public class MailNotification : Notification
{
    #region Fields

    private List<Attachment> _attachments;

    #endregion
    
    #region Properties
    public List<Attachment> Attachments { get => _attachments; }

    #endregion
    
    #region Constructor
    
    public MailNotification(NotificationInformationType type, string title, string message, List<Attachment> attachments) : base(type, title, message)
    {
        _attachments = attachments;
    }
    public MailNotification(NotificationInformationType type, string title, string message) : base(type, title, message)
    {
    }
    #endregion
    
    #region Methods
    
    public MailMessage ConvertToMailMessage()
    {
        var mailMessage = new MailMessage
        {
            Subject = CreateTitle(),
            Body = CreateBody(),
            IsBodyHtml = true,
            Priority = GetPriority()
        };
        if (_attachments != null)
        {
            mailMessage.Attachments.ToList().AddRange(_attachments);
        }
        return mailMessage;
    }

    #endregion

    #region Private Methods
    private string CreateTitle()
    {
        switch (Type)
        {
            case NotificationInformationType.AUTH:
                return "Авторизация " + Title;
            case NotificationInformationType.INFO:
                return "Информация "+ Title;
            case NotificationInformationType.WARNING:
                return "Предупреждение "+ Title;
            case NotificationInformationType.ERROR:
                return "Ошибка "+ Title;
            default:
                return "Информация "+ Title;
        }
    }

    private string CreateBody()
    {
        string formattedMessage;

        switch (Type)
        {
            case NotificationInformationType.AUTH:
                formattedMessage = "Вы успешно авторизовались.";
                break;
            case NotificationInformationType.INFO:
                formattedMessage = "Это информационное сообщение.";
                break;
            case NotificationInformationType.WARNING:
                formattedMessage = "Внимание! Обратите внимание на это предупреждение.";
                break;
            case NotificationInformationType.ERROR:
                formattedMessage = "Произошла ошибка. Пожалуйста, проверьте детали.";
                break;
            default:
                formattedMessage = "Сообщение не определено.";
                break;
        }

        return $"<p style='font-size: 16px; line-height: 1.5; color: #555;'>{formattedMessage} {Message}</p>";
    }
    
    private MailPriority GetPriority()
    {
        switch (Type)
        {
            case NotificationInformationType.AUTH:
                return MailPriority.High;
            case NotificationInformationType.INFO:
                return MailPriority.Normal;
            case NotificationInformationType.WARNING:
                return MailPriority.Low;
            case NotificationInformationType.ERROR:
                return MailPriority.High;
            default:
                return MailPriority.Normal;
        }
    }

    #endregion
}
