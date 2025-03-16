using MDSBackend.Models.Database;

namespace MDSBackend.Services.TFA;

public interface ITwoFactorService
{
    string GenerateTwoFactorSecretKey(ApplicationUser user);
    string GenerateTwoFactorCode(ApplicationUser user);
    bool ValidateTwoFactorCode(ApplicationUser user, string code);
    Task SendTwoFactorNotificationAsync(ApplicationUser user, string code);
}