namespace MDSBackend.Models.DTO;

public class LoginResultDTO
{
    public bool? RequiresTwoFactorAuth { get; set; }
    public bool Success { get; set; }
    public RefreshTokenDTO? Token { get; set; }
}