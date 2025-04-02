using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionCreateDTO
{
  public long? Id { get; set; }

  [Required(ErrorMessage = "Title is required")]
  public string Title { get; set; } = null!;

  public string? Description { get; set; }

  [Required(ErrorMessage = "Category id is required")]
  public long CategoryId { get; set; }

  /// <summary>
  /// If AssignDate is set, the instruction will be automatically enabled
  /// when the date is reached. If it's not set, the test will automatically
  /// obtain the current date as its AssignDate as soon as the instruction
  /// will be enabled by the IsEnabled parameter.
  /// </summary>
  public DateTime? AssignDate { get; set; }
  
  /// <summary>
  /// When deadline is reached, no more submissions are allowed for this instruction.
  /// </summary>
  public DateTime? DeadlineDate { get; set; }

  /// <summary>
  /// Disabled instructions cannot be seen by users.
  /// Tests for such instructions cannot be submitted either.
  /// </summary>
  public bool IsEnabled { get; set; } = false;
}
