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

  public IEnumerable<PostGetDTO> GetPostsByUsername(string username)
  {
    var postList = _context.Posts.Where(user => user.Username == username)
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

  public void UpdatePost(PostUpdateDTO post)
  {
    var postFound = _context.Posts.Find(post.Id);
    if (postFound is not null)
    {
      postFound.Image = string.IsNullOrEmpty(post.Image) ? postFound.Image : post.Image;
      postFound.Text = string.IsNullOrEmpty(post.Text) ? postFound.Text : post.Text;
      postFound.UpdatedAt = DateTime.Now;
      _context.SaveChanges();
    }
  }

  public void DeletePost(Guid postId)
  {
    var postFound = _context.Posts.Find(postId);
    if (postFound is not null)
    {
      _context.Posts.Remove(postFound);
      _context.SaveChanges();
    }
  }
}