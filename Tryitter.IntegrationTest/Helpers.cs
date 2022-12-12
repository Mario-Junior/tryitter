using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.IntegrationTest;

public static class Helpers
{
  public static List<User> GetUserListForTests() =>
      new() {
                new User{
                    Username = "test1",
                    Email = "test1@test.com",
                    Name = "test 1",
                    Password = "test1234",
                    Photo = "http://local.com/test1.jpg",
                    Module = "Computer Science",
                    Status = "testing 1",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test2",
                    Email = "test2@test.com",
                    Name = "test 2",
                    Password = "test123",
                    Photo = "http://local.com/test2.jpg",
                    Module = "Computer Science",
                    Status = "testing 2",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test3",
                    Email = "test3@test.com",
                    Name = "test 3",
                    Password = "test123",
                    Photo = "http://local.com/test3.jpg",
                    Module = "Computer Science",
                    Status = "testing 3",
                    CreatedAt = DateTime.Today
                },
      };

  public static List<Post> GetPostListForTests() =>
      new() {
                new Post {
                    Id = new Guid("123e4567e89b12d3a456426655440000"),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test1",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid("123e4567e89b12d3a456426655440001"),
                    Text = "Post 1",
                    Image = "http://local.com/post2.jpg",
                    Username = "test2",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
      };
}