using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.Models;
using Tryitter.Repository;

namespace Tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
  private readonly PostRepository _repository;

  public PostController(PostRepository repository)
  {
    _repository = repository;
  }

  [HttpPost]
  [Authorize(Policy = "AuthorizedUser")]
  public IActionResult CreatePost([FromBody] PostCreateDTO post)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != post.Username) return Forbid();
    
    return Created("", _repository.CreatePost(post));
  }

  [HttpGet("{postId}")]
  [AllowAnonymous]
  public IActionResult GetPostById(Guid postId)
  {
    var post = _repository.GetPostById(postId);

    if (post is null) return NotFound("Post not found");
    return Ok(post);
  }

  [HttpGet("{username}/{postId}")]
  [AllowAnonymous]
  public IActionResult GetPostByUsernameAndId(string username, Guid postId)
  {
    var post = _repository.GetPostByUsernameAndId(username, postId);

    if (post is null) return NotFound("Post not found");
    return Ok(post);
  }

  [HttpGet]
  [AllowAnonymous]
  public IActionResult GetAllPosts()
  {
    var postList = _repository.GetAllPosts();
    return Ok(postList);
  }

  [HttpPut("{username}")]
  [Authorize(Policy = "AuthorizedUser")]
  public IActionResult UpdatePost([FromBody] PostUpdateDTO post, string username)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != username) return Forbid();

    var postFound = _repository.GetPostByUsernameAndId(username, post.Id);

    if (postFound is null) return NotFound("Post not found");

    _repository.UpdatePost(post);
    return Ok("Post updated");
  }

  [HttpDelete("{username}")]
  [Authorize(Policy = "AuthorizedUser")]
  public IActionResult DeletePost([FromBody] Guid postId, string username)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != username) return Forbid();

    var postFound = _repository.GetPostByUsernameAndId(username, postId);

    if (postFound is null) return NotFound("Post not found");

    _repository.DeletePost(postId);
    return NoContent();
  }
}