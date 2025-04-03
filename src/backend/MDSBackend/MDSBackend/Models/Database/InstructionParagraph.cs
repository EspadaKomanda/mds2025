using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.Database;

public class InstructionParagraph
{
    [Key]
    public long Id { get; set; }

    public long InstructionId { get; set; }
    [Required(ErrorMessage = "Must be linked to instruction")]

    public int Order { get; set; }

    [Required(ErrorMessage = "Paragraph text is required")]
    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }
}
