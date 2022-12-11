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
    await _context.SaveChangesAsync();

    return new PostGetDTO {
      Id = newPost.Id,
      Text = newPost.Text,
      Image = newPost.Image,
      Username = newPost.Username,
      CreatedAt = newPost.CreatedAt,
      UpdatedAt = newPost.UpdatedAt,
    };
  }

  public async Task<IEnumerable<PostGetDTO>> GetPostsByUsername(string username)
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
    
    return await Task.FromResult(postList);
  }

  public async Task<PostGetDTO> GetPostByUsernameAndId(string username, Guid postId)
  {
    var post = _context.Posts.Where(user => user.Username == username)
      .Where(postd => postd.Id == postId).FirstOrDefault();
    if (post is null) return null!;

    return await Task.FromResult(
      new PostGetDTO {
      Id = post.Id,
      Text = post.Text,
      Image = post.Image,
      Username = post.Username,
      CreatedAt = post.CreatedAt,
      UpdatedAt = post.UpdatedAt,
      }
    );
  }

  public async Task<IEnumerable<PostGetDTO>> GetAllPosts()
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
    
    return await Task.FromResult(postList);
  }

  public async Task<bool> UpdatePost(PostUpdateDTO post)
  {
    var postFound = await _context.Posts.FindAsync(post.Id);
    if (postFound is not null)
    {
      postFound.Image = string.IsNullOrEmpty(post.Image) ? postFound.Image : post.Image;
      postFound.Text = string.IsNullOrEmpty(post.Text) ? postFound.Text : post.Text;
      postFound.UpdatedAt = DateTime.Now;
      await _context.SaveChangesAsync();
    }
    return true;
  }

  public async Task<bool> DeletePost(Guid postId)
  {
    var postFound = await _context.Posts.FindAsync(postId);
    if (postFound is not null)
    {
      await Task.Run(() => _context.Posts.Remove(postFound));
      await _context.SaveChangesAsync();
    }
    return true;
  }
}