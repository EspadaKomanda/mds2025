using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class TwoFactorLoginDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be 6 characters long")]
    public string Code { get; set; } = null!;
}