namespace MDSBackend.Models.DTO;

public class RefreshTokenDTO
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}