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
                    Password = "test1234",
                    Photo = "http://local.com/test2.jpg",
                    Module = "Computer Science",
                    Status = "testing 2",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test3",
                    Email = "test3@test.com",
                    Name = "test 3",
                    Password = "test1234",
                    Photo = "http://local.com/test3.jpg",
                    Module = "Computer Science",
                    Status = "testing 3",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test4",
                    Email = "test4@test.com",
                    Name = "test 4",
                    Password = "test1234",
                    Photo = "http://local.com/test4.jpg",
                    Module = "Computer Science",
                    Status = "testing 4",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test5",
                    Email = "test5@test.com",
                    Name = "test create post",
                    Password = "test1234",
                    Photo = "http://local.com/test5.jpg",
                    Module = "Computer Science",
                    Status = "testing 5",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test6",
                    Email = "test6@test.com",
                    Name = "test get PostByUsername",
                    Password = "test1234",
                    Photo = "http://local.com/test6.jpg",
                    Module = "Computer Science",
                    Status = "testing 6",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test7",
                    Email = "test7@test.com",
                    Name = "test get PostByUsernameAndId",
                    Password = "test1234",
                    Photo = "http://local.com/test7.jpg",
                    Module = "Computer Science",
                    Status = "testing 7",
                    CreatedAt = DateTime.Today
                },
                new User{
                    Username = "test8",
                    Email = "test8@test.com",
                    Name = "test update post",
                    Password = "test1234",
                    Photo = "http://local.com/test8.jpg",
                    Module = "Computer Science",
                    Status = "testing 8",
                    CreatedAt = DateTime.Today
                },
      };

  public static List<Post> GetPostListForTests() =>
      new() {
                new Post {
                    Id = new Guid("123e4567e89b12d3a456426655440000"),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test6",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid("123e4567e89b12d3a456426655440001"),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test7",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid("123e4567e89b12d3a456426655440002"),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test8",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
      };
}