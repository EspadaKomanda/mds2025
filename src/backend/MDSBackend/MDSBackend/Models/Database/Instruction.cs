using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class Instruction
{
  [Key]
  public long Id { get; set; }

  [Required(ErrorMessage = "Title is required")]
  public string Title { get; set; } = null!;

  public string? Description { get; set; }

  public long CategoryId { get; set; }
  [Required(ErrorMessage = "Category must be specified")]
  public InstructionCategory Category { get; set; } = null!;

  public DateTime? AssignDate { get; set; } = DateTime.UtcNow;
  
  public DateTime? DeadlineDate { get; set; }
}
