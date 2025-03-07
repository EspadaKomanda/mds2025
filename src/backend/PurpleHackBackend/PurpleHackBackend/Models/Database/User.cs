using System.ComponentModel.DataAnnotations;

namespace PurpleHackBackend.Models.Database;

public class User
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username must be less than 50 characters")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password must be less than 100 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public Role Role { get; set; } = null!;
    public long RoleId { get; set; }
}