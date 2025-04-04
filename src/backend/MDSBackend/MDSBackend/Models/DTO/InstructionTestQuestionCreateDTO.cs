using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionTestQuestionCreateDTO
{
  public long? Id { get; set; }

  public bool IsMultipleAnswer { get; set; }

  /// <summary>
  /// Question will be displayed in the paragraph with the same order number.
  /// There can be multiple questions attached to the same paragraph.
  /// </summary>
  public int Order { get; set; }

  [Required(ErrorMessage = "Must have question text")]
  public string Question { get; set; } = null!;

  [Required(ErrorMessage = "Must have answer options")]
  public ICollection<string> Answers { get; set; } = null!;

  [Required(ErrorMessage = "Must have correct answers")]
  public ICollection<int> CorrectAnswers { get; set; } = null!;
} 

