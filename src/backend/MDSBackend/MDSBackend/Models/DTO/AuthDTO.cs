using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class AuthDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; } = null!;
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;
    
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    public string Password { get; set; } = null!;
    
    [Required]
    public bool RememberMe { get; set; }
}