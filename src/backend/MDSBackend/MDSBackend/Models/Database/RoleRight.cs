namespace MDSBackend.Models.Database;

public class RoleRight
{
    public long RoleId { get; set; }
    public ApplicationRole Role { get; set; } = null!;
    
    public long RightId { get; set; }
    public Right Right { get; set; } = null!;
}