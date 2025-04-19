using System.ComponentModel.DataAnnotations;
using MDSBackend.Utils;
using Microsoft.AspNetCore.Identity;

namespace MDSBackend.Models.Database;

public class ApplicationUser : IdentityUser<long>
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username must be less than 50 characters")]
    public string Username { get; set; } = null!;
    public bool TwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }
    public bool EmailConfirmed { get; set; }
    public List<TwoFactorProvider> TwoFactorProviders { get; set; } = new List<TwoFactorProvider>(); 
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
}