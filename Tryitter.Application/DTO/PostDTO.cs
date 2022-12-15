using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tryitter.DTO;

public class PostCreateDTO
{
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Text { get; set; } = default!;
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Image { get; set; }
  [ForeignKey("Username")]
  [MaxLength(15, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Username { get; set; } = default!;
}

public class PostGetDTO
{
  public Guid Id { get; set; }
  public string Text { get; set; } = default!;
  public string? Image { get; set; }
  public string Username { get; set; } = default!;
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class PostUpdateDTO
{
  public Guid Id { get; set; }
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Text { get; set; }
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Image { get; set; }
}