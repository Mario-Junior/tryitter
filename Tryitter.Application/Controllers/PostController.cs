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
}