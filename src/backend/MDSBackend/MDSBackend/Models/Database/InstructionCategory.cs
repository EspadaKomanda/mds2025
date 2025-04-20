using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class InstructionCategory
{
  [Key]
  public long Id { get; set; }

  [Required(ErrorMessage = "Title is required")]
  public string Title { get; set; } = null!;
}

