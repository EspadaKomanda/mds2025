using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionTestQuestionDTO
{
  public long? Id { get; set; }

  public bool IsMultipleAnswer { get; set; }

  [Required(ErrorMessage = "Must have question text")]
  public string Question { get; set; } = null!;

  [Required(ErrorMessage = "Must have answer options")]
  public ICollection<string> Answers { get; set; } = null!;

  public ICollection<int>? CorrectAnswers { get; set; }
} 

