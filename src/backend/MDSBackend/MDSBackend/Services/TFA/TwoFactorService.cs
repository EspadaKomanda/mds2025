using System.Security.Cryptography;
using MDSBackend.Models.Database;
using MDSBackend.Services.NotificationService;
using MDSBackend.Utils;
using MDSBackend.Utils.Factory;
using Microsoft.Extensions.Options;
using OtpNet;

namespace MDSBackend.Services.TFA;

public class TwoFactorService : ITwoFactorService
{
    #region Services

    private readonly ILogger<ITwoFactorService> _logger;
    private readonly INotificationService _notificationService;
    private readonly MailNotificationsFactory _mailNotificationsFactory;
    private readonly PushNotificationsFactory _pushNotificationsFactory;
    #endregion
    
    #region Constructor
    
    public TwoFactorService(ILogger<ITwoFactorService> logger, MailNotificationsFactory mailNotificationsFactory, INotificationService notificationService, PushNotificationsFactory pushNotificationsFactory)
    {
        _logger = logger;
        _mailNotificationsFactory = mailNotificationsFactory;
        _notificationService = notificationService;
        _pushNotificationsFactory = pushNotificationsFactory;
    }
    
    #endregion
    
    public string GenerateTwoFactorSecretKey(ApplicationUser user)
    {
        var key = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        var secretKey = Convert.ToBase64String(key);
        user.TwoFactorSecret = secretKey;
        return secretKey;
    }
    
    public bool ValidateTwoFactorCode(ApplicationUser user, string code)
    {
        if (string.IsNullOrEmpty(user.TwoFactorSecret))
        {
            return false;
        }
        
        var secretKeyBytes = Convert.FromBase64String(user.TwoFactorSecret);
        
        var totp = new Totp(secretKeyBytes);
        return totp.VerifyTotp(code, out long _);
    }

    public async Task SendTwoFactorNotificationAsync(ApplicationUser user, string code)
    {
        try
        {
            var message = $"Your verification code is: {code}";
            var title = "Verification Code";
            await _notificationService.SendMailNotificationAsync(user, _mailNotificationsFactory.CreateNotification(NotificationInformationType.AUTH, title, message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public string GenerateTwoFactorCode(ApplicationUser user)
    {
        if (string.IsNullOrEmpty(user.TwoFactorSecret))
        {
            throw new InvalidOperationException("User does not have a two-factor secret key");
        }
       
        var secretKeyBytes = Convert.FromBase64String(user.TwoFactorSecret);
        var totp = new Totp(secretKeyBytes);
        return totp.ComputeTotp();
    }
}