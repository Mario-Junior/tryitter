using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Models;
using Tryitter.Repository;
using Tryitter.Auth;

namespace Tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  private readonly UserRepository _repository;

  public UserController(UserRepository repository)
  {
    _repository = repository;
  }

  [HttpPost]
  [AllowAnonymous]
  public IActionResult CreateUser([FromBody] UserCreateDTO user)
  {
    var userFound = _repository.GetUserByUsername(user.Username);

    if (userFound is not null)
      return BadRequest("User already exists with this username");
      
    return Created("", _repository.CreateUser(user));
  }

  [HttpPost("login")]
  [AllowAnonymous]
  public IActionResult Login([FromBody] UserLoginDTO user)
  {
    var userFound = _repository.LoginValidate(user.Username, user.Password);

    if (userFound is null)
      return NotFound("Username and/or password invalid");
    
    TokenGenerator tokenGenerator = new();

    string token = tokenGenerator.Generate(userFound);

    return Ok(token);
  }

  [HttpGet("{username}")]
  [AllowAnonymous]
  public IActionResult GetUserByUsername(string username)
  {
    var user = _repository.GetUserByUsername(username);

    if (user is null) return NotFound("Username not found");
    return Ok(user);
  }

  [HttpGet]
  [AllowAnonymous]
  public IActionResult GetAllUsers()
  {
    var userList = _repository.GetAllUsers();
    return Ok(userList);
  }

  [HttpPut]
  [Authorize(Policy = "AuthorizedUser")]
  public IActionResult UpdateUser([FromBody] UserUpdateDTO user)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != user.Username) return Forbid();

    _repository.UpdateUser(user);
    return Ok("User updated");
  }
  
  [HttpDelete]
  [Authorize(Policy = "AuthorizedUser")]
  public IActionResult DeleteUser([FromBody] string username)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != username) return Forbid();

    _repository.DeleteUser(username);
    return NoContent();
  }
}