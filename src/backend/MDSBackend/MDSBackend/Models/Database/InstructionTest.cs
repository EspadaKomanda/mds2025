using System.ComponentModel.DataAnnotations;
using MDSBackend.Utils.Enums;

namespace MDSBackend.Models.Database;

public class InstructionTest
{
  [Key]
  public long Id { get; set; }

  // Reserved just in case
  [MaxLength(300, ErrorMessage = "Title cannot be longer than 300 characters")]
  public string? Title { get; set; }

  public virtual ICollection<InstructionTestQuestion> Questions { get; set; }

  public int MaxAttempts { get; set; } = 10;

  public double MinScore { get; set; } = 0.6;

  public InstructionTestScoreCalcMethod ScoreCalcMethod { get; set; } = InstructionTestScoreCalcMethod.MaxGrade;

} 
