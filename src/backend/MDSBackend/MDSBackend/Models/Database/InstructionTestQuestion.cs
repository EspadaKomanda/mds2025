using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class InstructionTestQuestion
{
  [Key]
  public long Id { get; set; }

  [Required(ErrorMessage = "Must be tied to an instruction test")]
  public InstructionTest InstructionTest { get; set; } = null!;
  public long InstructionTestId { get; set; }

  public bool IsMultipleAnswer { get; set; }

  [Required(ErrorMessage = "Must have question text")]
  public string Question { get; set; } = null!;

  [Required(ErrorMessage = "Must have answer options")]
  public ICollection<string> Answers { get; set; } = null!;

  [Required(ErrorMessage = "Must have correct answer ids")]
  public ICollection<int> CorrectAnswers { get; set; } = null!;
} 
