using System.Net.Mail;
using MDSBackend.Models.Database;
using MDSBackend.Utils;
using MDSBackend.Utils.Factory;

namespace MDSBackend.Services.NotificationService;

public class NotificationService : INotificationService
{
    #region Services

    private readonly EmailClient _emailClient;
    private readonly MailNotificationsFactory _mailNotificationsFactory;
    private readonly ILogger<NotificationService> _logger;
    private readonly PushNotificationsClient _pushNotificationsClient;
    private readonly PushNotificationsFactory _pushNotificationsFactory;
    
    #endregion

    #region Constructor

    public NotificationService(EmailClient emailClient,  PushNotificationsClient pushNotificationsClient, ILogger<NotificationService> logger, MailNotificationsFactory mailNotificationsFactory, PushNotificationsFactory pushNotificationsFactory)
    {
        _emailClient = emailClient;
        _pushNotificationsClient = pushNotificationsClient;
        _logger = logger;
        _mailNotificationsFactory = mailNotificationsFactory;
        _pushNotificationsFactory = pushNotificationsFactory;
    }

    #endregion


    public async Task SendMailNotificationAsync(ApplicationUser user, string title, string message,
        NotificationInformationType notificationInformationType)
    {
        try
        {
            var notification = _mailNotificationsFactory.CreateNotification(notificationInformationType, title, message);
            await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }

    public async Task SendPushNotificationAsync(ApplicationUser user, string title, string message,
        NotificationInformationType notificationInformationType)
    {
        try
        {
            var notification = _pushNotificationsFactory.CreateNotification(notificationInformationType, title, message);
            await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }

    public async Task SendPushNotificationAsync(ApplicationUser user, string title, string message,
        NotificationInformationType notificationInformationType, string image)
    {
        try
        {
            var notification = _pushNotificationsFactory.CreateNotification(notificationInformationType, title, message,image);
            await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }

    public async Task SendPushNotificationAsync(ApplicationUser user, string title, string message,
        NotificationInformationType notificationInformationType, string? image, string clickAction, ClickActionType actionType)
    {
        try
        {
            var notification = _pushNotificationsFactory.CreateNotification(notificationInformationType, title, message,image,clickAction,actionType);
            await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }
}