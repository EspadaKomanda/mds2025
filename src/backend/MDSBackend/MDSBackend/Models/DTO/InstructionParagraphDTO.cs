namespace MDSBackend.Models.DTO;

public class InstructionParagraphDTO
{
    public long? Id { get; set; }

    public long? InstructionId { get; set; }

    /// <summary>
    /// Order defines the order of the paragraphs inside the instruction.
    /// There must not be two paragraphs with the same order.
    /// </summary>
    public int Order { get; set; }

    public string? Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }
}
 
