using System.ComponentModel.DataAnnotations;

namespace PurpleHackBackend.Models.DTO;

public class UserProfileDTO
{
    [Required(ErrorMessage = "Id is required")]
    public long Id { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public long UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(100, ErrorMessage = "Surname must be less than 100 characters")]
    public string Surname { get; set; } = null!;

    [StringLength(50, ErrorMessage = "Patronymic must be less than 50 characters")]
    public string? Patronymic { get; set; }

    [Required(ErrorMessage = "Birthdate is required")]
    public DateTime Birthdate { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage = "Invalid contact phone number")]
    public string? ContactPhone { get; set; }

    [Url(ErrorMessage = "Invalid avatar url")]
    public string? ProfilePicture { get; set; }
}
