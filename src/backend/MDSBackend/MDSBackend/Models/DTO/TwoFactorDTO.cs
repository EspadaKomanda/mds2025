using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class TwoFactorDTO
{
    [Required]
    public int TwoFactorProvider { get; set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string? Username { get; set; } = null!;

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be 6 characters long")]
    public string Code { get; set; } = null!;
    public bool RememberMe { get; set; }
}