namespace MDSBackend.Models.DTO;

public class DisableTwoFactorDTO
{
    public int TwoFactorProvider { get; set; }
    public string Code { get; set; }
}