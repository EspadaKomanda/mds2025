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

  public int Score { get; set; }
}
