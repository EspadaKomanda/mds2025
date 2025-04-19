using System.Net.Mail;
using MDSBackend.Models.Database;
using MDSBackend.Utils;
using MDSBackend.Utils.Factory;

namespace MDSBackend.Services.NotificationService;

public class NotificationService : INotificationService
{
    #region Services

    private readonly EmailClient _emailClient;
    private readonly ILogger<NotificationService> _logger;
    private readonly PushNotificationsClient _pushNotificationsClient;
    
    #endregion

    #region Constructor

    public NotificationService(EmailClient emailClient,  PushNotificationsClient pushNotificationsClient, ILogger<NotificationService> logger)
    {
        _emailClient = emailClient;
        _pushNotificationsClient = pushNotificationsClient;
        _logger = logger;
    }

    #endregion


    public async Task SendMailNotificationAsync(ApplicationUser user, Notification notification)
    {
        try
        {
            await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }

    //TODO: Refactor, add reg.ru notifications
    public async Task SendPushNotificationAsync(ApplicationUser user, Notification notification)
    {
        try
        {
            await _emailClient.SendEmail(((MailNotification)notification).ConvertToMailMessage(), user.Email);
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            throw;
        }
    }
}