using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class UserDTO
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username must be less than 50 characters")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;
    
    [StringLength(100, ErrorMessage = "Password must be less than 100 characters")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    public RoleDTO? Role { get; set; }
}