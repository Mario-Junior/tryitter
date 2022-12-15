using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tryitter.Models;

public class Post
{
  [Key]
  public Guid Id { get; set; }
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Text { get; set; } = default!;
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Image { get; set; }
  [ForeignKey("Username")]
  [MaxLength(15, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Username { get; set; } = default!;
  public User User { get; set; } = default!;
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public DateTime UpdatedAt { get; set; } = DateTime.Now;
}