using System.ComponentModel.DataAnnotations;

namespace PurpleHackBackend.Models.Database;

public class Role : AuditableEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
    public string Name { get; set; } = null!;
    
}