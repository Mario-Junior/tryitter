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
  public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO user)
  {
    var userFound = await _repository.GetUserByUsername(user.Username);

    if (userFound is not null)
      return BadRequest("User already exists with this username");
      
    return Created("", await _repository.CreateUser(user));
  }

  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
  {
    var userFound = await _repository.LoginValidate(user.Username, user.Password);

    if (userFound is null)
      return NotFound("Username and/or password invalid");
    
    TokenGenerator tokenGenerator = new();

    string token = tokenGenerator.Generate(userFound);

    return Ok($"Bearer {token}");
  }

  [HttpGet("{username}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetUserByUsername(string username)
  {
    var user = await _repository.GetUserByUsername(username);

    if (user is null) return NotFound("Username not found");
    return Ok(user);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetAllUsers()
  {
    var userList = await _repository.GetAllUsers();
    return Ok(userList);
  }

  [HttpPut]
  [Authorize(Policy = "AuthorizedUser")]
  public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO user)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != user.Username) return Forbid();

    await _repository.UpdateUser(user);
    return Ok("User updated");
  }
  
  [HttpDelete("{username}")]
  [Authorize(Policy = "AuthorizedUser")]
  public async Task<IActionResult> DeleteUser(string username)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != username) return Forbid();

    await _repository.DeleteUser(authenticatedUsername);
    return NoContent();
  }
}