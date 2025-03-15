using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class RoleDTO
{
    public long? Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
    public string Name { get; set; } = null!;
    
    public DateTimeOffset? CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }
}