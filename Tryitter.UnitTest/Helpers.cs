using Microsoft.EntityFrameworkCore;
using Tryitter.Models;
using Tryitter.Repository;

namespace Tryitter.UnitTest;

public static class Helpers
{
  public static TryitterContext GetContextInstanceForTests(string inMemoryDbName)
  {
    var contextOptions = new DbContextOptionsBuilder<TryitterContext>()
        .UseInMemoryDatabase(inMemoryDbName)
        .Options;
    var context = new TryitterContext(contextOptions);
    context.Users.AddRange(
        GetUserListForTests()
    );
    context.Posts.AddRange(
        GetPostListForTests()
    );
    context.SaveChanges();
    return context;
  }

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
                    Password = "test1234",
                    Photo = "http://local.com/test3.jpg",
                    Module = "Computer Science",
                    Status = "testing 3",
                    CreatedAt = DateTime.Today
                },
      };

  public static List<Post> GetPostListForTests() =>
      new() {
                new Post {
                    Id = new Guid(),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test1",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid(),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test1",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid(),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test1",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid(),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test1",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid(),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test2",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
                new Post {
                    Id = new Guid(),
                    Text = "Post 1",
                    Image = "http://local.com/post1.jpg",
                    Username = "test2",
                    CreatedAt = DateTime.Today,
                    UpdatedAt = DateTime.Today
                },
      };
}
