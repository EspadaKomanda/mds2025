using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class InstructionTestResult
{
  [Key]
  public long Id { get; set; }

  public long InstructionTestId { get; set; }
  [Required(ErrorMessage = "Instruction test is required")]
  public InstructionTest InstructionTest { get; set; } = null!;

  public long UserId { get; set; }
  [Required(ErrorMessage = "User is required")]
  public ApplicationUser User { get; set; } = null!;

  [Range(0, 100, ErrorMessage = "Score must be a number from 0 to 100")]
  public int Score { get; set; }
}
