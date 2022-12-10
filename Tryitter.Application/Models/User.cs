using System.ComponentModel.DataAnnotations;

namespace Tryitter.Models;

public class User : UserCreateDTO
{
  public DateTime CreatedAt { get; set; } = DateTime.Now;
  public ICollection<Post> Posts { get; set; } = default!;
}

public class UserCreateDTO
{
  [Key]
  [MaxLength(15, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Username { get; set; } = default!;
  [MaxLength(100, ErrorMessage = "Maximum of {1} characters allowed")]
  [EmailAddress]
  [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$", ErrorMessage="Invalid Email format")]
  public string Email { get; set; } = default!;
  [MaxLength(50, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Name { get; set; } = default!;
  [MaxLength(50, ErrorMessage = "Maximum of {1} characters allowed")]
  public string Password { get; set; } = default!;
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Photo { get; set; }
  [MaxLength(50, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Module { get; set; }
  [MaxLength(300, ErrorMessage = "Maximum of {1} characters allowed")]
  public string? Status { get; set; }
}

public class UserGetDTO
{
  public string Username { get; set; } = default!;
  public string Email { get; set; } = default!;
  public string Name { get; set; } = default!;
  public string? Photo { get; set; }
  public string? Module { get; set; }
  public string? Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public ICollection<PostGetDTO> Posts { get; set; } = default!;
}

public class UserLoginDTO
{
  public string Username { get; set; } = default!;
  public string Password { get; set; } = default!;
}