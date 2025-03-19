using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionTestCreateDTO
{
  public string? Title { get; set; }

  [Required(ErrorMessage = "Questions must be specified")]
  public ICollection<InstructionTestQuestionCreateDTO> Questions { get; set; } = null!;
}
