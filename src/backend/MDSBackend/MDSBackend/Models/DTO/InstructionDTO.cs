namespace MDSBackend.Models.DTO;

public class InstructionDTO
{
  public long? Id { get; set; }

  public string? Title { get; set; }

  public string? Description { get; set; }

  public long? CategoryId { get; set; }

  public DateTime? AssignDate { get; set; }
  
  public DateTime? DeadlineDate { get; set; }
}

