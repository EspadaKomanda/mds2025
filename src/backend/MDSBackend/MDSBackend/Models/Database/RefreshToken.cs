using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class RefreshToken
{
    [Key]
    public long Id { get; set; }
    
    public long UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    
    [Required]
    public string Token { get; set; } = null!;
    
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    
    public bool IsRevoked { get; set; }
    public string? RevokedByIp { get; set; }
    public DateTime? RevokedOn { get; set; }
    
    public bool IsActive => !IsRevoked && !IsExpired;
}