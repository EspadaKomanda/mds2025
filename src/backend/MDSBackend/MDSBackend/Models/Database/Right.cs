using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class Right
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
    
    [StringLength(100)]
    public string? Description { get; set; }
    
    public List<RoleRight> RoleRights { get; set; } = new List<RoleRight>();
}