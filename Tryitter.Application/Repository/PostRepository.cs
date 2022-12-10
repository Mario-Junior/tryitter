using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tryitter.Models;

namespace Tryitter.Repository;

public class PostRepository
{
  protected readonly TryitterContext _context;

  public PostRepository(TryitterContext context)
  {
    _context = context;
  }

  public async Task<PostGetDTO> CreatePost(PostCreateDTO post)
  {
    var newPost = new Post {
      Text = post.Text,
      Image = post.Image,
      Username = post.Username,
      CreatedAt = DateTime.Now,
      UpdatedAt = DateTime.Now,
    };
    await _context.Posts.AddAsync(newPost);
    _context.SaveChanges();

    return new PostGetDTO {
      Id = newPost.Id,
      Text = newPost.Text,
      Image = newPost.Image,
      Username = newPost.Username,
      CreatedAt = newPost.CreatedAt,
      UpdatedAt = newPost.UpdatedAt,
    };
  }

  public PostGetDTO GetPostById(Guid postId)
  {
    Post post = _context.Posts.Find(postId)!;
    if (post is null) return null!;

    return new PostGetDTO {
      Id = post.Id,
      Text = post.Text,
      Image = post.Image,
      Username = post.Username,
      CreatedAt = post.CreatedAt,
      UpdatedAt = post.UpdatedAt,
    };
  }

  public PostGetDTO GetPostByUsernameAndId(string username, Guid postId)
  {
    var post = _context.Posts.Where(user => user.Username == username)
      .Where(postd => postd.Id == postId).FirstOrDefault();
    if (post is null) return null!;

    return new PostGetDTO {
      Id = post.Id,
      Text = post.Text,
      Image = post.Image,
      Username = post.Username,
      CreatedAt = post.CreatedAt,
      UpdatedAt = post.UpdatedAt,
    };
  }

  public IEnumerable<PostGetDTO> GetAllPosts()
  {
    var postList = _context.Posts
      .Select(p => new PostGetDTO
      {
        Id = p.Id,
        Text = p.Text,
        Image = p.Image,
        Username = p.Username,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
      }).ToList();
    
    return postList;
  }
}