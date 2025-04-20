using System.ComponentModel.DataAnnotations;
using MDSBackend.Utils.Enums;

namespace MDSBackend.Models.DTO;

public class InstructionTestCreateDTO
{
  public long? Id { get; set; }
  public string? Title { get; set; }

  [Required(ErrorMessage = "Questions must be specified")]
  public ICollection<InstructionTestQuestionCreateDTO> Questions { get; set; } = null!;

  public int MaxAttempts { get; set; } = 10;

  [Range(0, 1.0, ErrorMessage = "Minimum score must be between 0.6 and 1.0")]
  public double MinScore { get; set; } = 0.6;

  public InstructionTestScoreCalcMethod ScoreCalcMethod { get; set; } = InstructionTestScoreCalcMethod.MaxGrade; 
}
