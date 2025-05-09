using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionTestSubmissionDTO
{
  [Required(ErrorMessage = "InstructionTestId is required")]
  public int InstructionTestId { get; set; } 

  [Required(ErrorMessage = "Answers must be provided")]
  public List<List<int>> Answers { get; set; } = null!;   
}
