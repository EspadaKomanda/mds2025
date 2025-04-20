namespace MDSBackend.Models.DTO;

public class InstructionDTO
{
  public long? Id { get; set; }

  public string? Title { get; set; }

  public string? Description { get; set; }

  public List<InstructionParagraphDTO> Paragraphs { get; set; } = new List<InstructionParagraphDTO>();

  public long? CategoryId { get; set; }

  public DateTime? AssignDate { get; set; }
  
  public DateTime? DeadlineDate { get; set; }

  public bool IsEnabled { get; set; }
}

