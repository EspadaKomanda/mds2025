namespace MDSBackend.Models.DTO;

public class InstructionTestResultDTO
{
  public long? Id { get; set; }

  public long? InstructionTestId { get; set; }

  public long? UserId { get; set; }

  public int Score { get; set; }
}
