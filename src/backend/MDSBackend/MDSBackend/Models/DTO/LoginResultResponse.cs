namespace MDSBackend.Models.DTO;

public class LoginResultResponse
{
    public bool? RequiresTwoFactorAuth { get; set; }
    public bool Success { get; set; }
    public RefreshTokenDTO? Token { get; set; }
    public int? TwoFactorProvider { get; set; }
}