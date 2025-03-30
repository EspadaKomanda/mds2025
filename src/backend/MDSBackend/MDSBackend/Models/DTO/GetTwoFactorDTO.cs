using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class GetTwoFactorDTO
{
    [Required]
    public int TwoFactorProvider { get; set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string? Username { get; set; } = null!;
}