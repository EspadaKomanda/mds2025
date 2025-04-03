using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionParagraphCreateDTO
{
    public long? Id { get; set; }

    /// <summary>
    /// Order defines the order of the paragraphs inside the instruction.
    /// There must not be two paragraphs with the same order.
    /// </summary>
    public int Order { get; set; }

    [Required(ErrorMessage = "Text is required")]
    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }
}
 

