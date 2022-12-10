using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tryitter.Models;

namespace Tryitter.Repository;

public class UserRepository
{
  protected readonly TryitterContext _context;

  public UserRepository(TryitterContext context)
  {
    _context = context;
  }

  public async Task<UserGetDTO> CreateUser(UserCreateDTO user)
  {
    var newUser = new User {
      Username = user.Username,
      Email = user.Email,
      Name = user.Name,
      Password = user.Password,
      Photo = user.Photo,
      Module = user.Module,
      Status = user.Status,
      CreatedAt = DateTime.Now,
    };
    await _context.Users.AddAsync(newUser);
    _context.SaveChanges();
    return new UserGetDTO {
      Username = user.Username,
      Email = user.Email,
      Name = user.Name,
      Photo = user.Photo,
      Module = user.Module,
      Status = user.Status,
      CreatedAt = newUser.CreatedAt,
    };
  }

  public User LoginValidate(string username, string password)
  {
    var userFound = _context.Users.Find(username);
    if (userFound is null || userFound.Password != password)
      return null!;
    return userFound;
  }

  public UserGetDTO GetUserByUsername(string username)
  {
    var user = _context.Users.Where(user => user.Username == username)
      .Select(u => new UserGetDTO
      {
        Username = u.Username,
        Email = u.Email,
        Name = u.Name,
        Photo = u.Photo,
        Module = u.Module,
        Status = u.Status,
        CreatedAt = u.CreatedAt,
        Posts = u.Posts.Select(p => new PostGetDTO
        {
          Id = p.Id,
          Text = p.Text,
          Image = p.Image,
          Username = p.Username,
          CreatedAt = p.CreatedAt,
          UpdatedAt = p.UpdatedAt
        }).ToList()}).FirstOrDefault();

    if(user is null) return null!;
    return user;
  }

  public IEnumerable<UserGetAllDTO> GetAllUsers()
  {
    var userList = _context.Users
      .Select(u => new UserGetAllDTO
      {
        Username = u.Username,
        Name = u.Name,
        Photo = u.Photo,
        Module = u.Module,
        Status = u.Status,
        Posts = u.Posts.Select(p => new PostGetDTO
        {
          Id = p.Id,
          Text = p.Text,
          Image = p.Image,
          Username = p.Username,
          CreatedAt = p.CreatedAt,
          UpdatedAt = p.UpdatedAt
        }).ToList()
      });
    
    return userList;
  }

  public void UpdateUser(UserUpdateDTO user)
  {
    var userFound = _context.Users.Find(user.Username);
    if (userFound is not null)
    {
      userFound.Email = String.IsNullOrEmpty(user.Email) ? userFound.Email : user.Email;
      userFound.Module = String.IsNullOrEmpty(user.Module) ? userFound.Module : user.Module;
      userFound.Name = String.IsNullOrEmpty(user.Name) ? userFound.Name : user.Name;
      userFound.Password = String.IsNullOrEmpty(user.Password) ? userFound.Password : user.Password;
      userFound.Photo = String.IsNullOrEmpty(user.Photo) ? userFound.Photo : user.Photo;
      userFound.Status = String.IsNullOrEmpty(user.Status) ? userFound.Status : user.Status;
      _context.SaveChanges();
    }
  }

  public void DeleteUser(string username)
  {
    var userFound = _context.Users.Find(username);
    if (userFound is not null)
    {
      _context.Users.Remove(userFound);
      _context.SaveChanges();
    }
  }
}