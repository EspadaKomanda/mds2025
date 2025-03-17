using System.Security.Cryptography;
using MDSBackend.Models.Database;
using MDSBackend.Utils;
using Microsoft.Extensions.Options;
using OtpNet;

namespace MDSBackend.Services.TFA;

public class TwoFactorService : ITwoFactorService
{
    #region Services

    private readonly ILogger<ITwoFactorService> _logger;
    private readonly NotificationsFactory _notificationsFactory;
    
    #endregion
    
    #region Constructor
    
    public TwoFactorService(ILogger<ITwoFactorService> logger, 
        NotificationsFactory notificationsFactory)
    {
        _logger = logger;
        _notificationsFactory = notificationsFactory;
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