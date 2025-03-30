namespace MDSBackend.Models.Database;

public class UserRole
{
    public long UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    
    public long RoleId { get; set; }
    public ApplicationRole Role { get; set; } = null!;
}