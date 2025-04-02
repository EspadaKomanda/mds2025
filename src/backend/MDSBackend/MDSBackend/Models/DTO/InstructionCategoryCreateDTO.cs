using System.ComponentModel.DataAnnotations;

namespace MDSBackend.Models.DTO;

public class InstructionCategoryCreateDTO
{
  public long? Id { get; set; }

  [Required(ErrorMessage = "Title is required")]
  public string Title { get; set; } = null!;
}



