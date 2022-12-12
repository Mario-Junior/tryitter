using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tryitter.DTO;
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
  public async Task<IActionResult> CreatePost([FromBody] PostCreateDTO post)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != post.Username) return Forbid();
    
    return Created("", await _repository.CreatePost(post));
  }

  [HttpGet("{username}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetPostsByUsername(string username)
  {
    var postList = await _repository.GetPostsByUsername(username);
    if (!postList.Any()) return NotFound("Posts of this username is not found");
    return Ok(postList);
  }

  [HttpGet("{username}/{postId}")]
  [AllowAnonymous]
  public async Task<IActionResult> GetPostByUsernameAndId(string username, Guid postId)
  {
    var post = await _repository.GetPostByUsernameAndId(username, postId);

    if (post is null) return NotFound("Post not found");
    return Ok(post);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> GetAllPosts()
  {
    var postList = await _repository.GetAllPosts();
    return Ok(postList);
  }

  [HttpPut("{username}")]
  [Authorize(Policy = "AuthorizedUser")]
  public async Task<IActionResult> UpdatePost([FromBody] PostUpdateDTO post, string username)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != username) return Forbid();

    var postFound = await _repository.GetPostByUsernameAndId(username, post.Id);

    if (postFound is null) return NotFound("Post not found");

    await _repository.UpdatePost(post);
    return Ok("Post updated");
  }

  [HttpDelete("{username}/{postId}")]
  [Authorize(Policy = "AuthorizedUser")]
  public async Task<IActionResult> DeletePost(string username, Guid postId)
  {
    var authenticatedUsername = User.Identity!.Name;
    if (authenticatedUsername != username) return Forbid();

    var postFound = await _repository.GetPostByUsernameAndId(username, postId);

    if (postFound is null) return NotFound("Post not found");

    await _repository.DeletePost(postId);
    return NoContent();
  }
}