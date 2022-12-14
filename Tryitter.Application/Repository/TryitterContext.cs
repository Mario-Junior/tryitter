using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.Repository;

public class TryitterContext : DbContext
{
  public TryitterContext(DbContextOptions<TryitterContext> options) : base(options) {}

  public TryitterContext() {}

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<Post> Posts { get; set; } = null!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      var connectionString = "Server=127.0.0.1;Database=tryitter;User=SA;Password=123@SQLServer;TrustServerCertificate=True";
      optionsBuilder.UseSqlServer(connectionString);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Post>()
      .HasOne(p => p.User)
      .WithMany(u => u.Posts)
      .HasForeignKey(p => p.Username);

    modelBuilder.Entity<User>().HasData(
      new User {
        Username = "user1",
        Email = "user1@test.com",
        Name = "User 1",
        Password = "user1234",
        Photo = "http://local.com/user1.jpg",
        Module = "Computer Science",
        Status = "Using Tryitter",
        CreatedAt = DateTime.Today
      },
      new User {
        Username = "user2",
        Email = "user2@test.com",
        Name = "User 2",
        Password = "user1234",
        Photo = "http://local.com/user2.jpg",
        Module = "Computer Science",
        Status = "Using Tryitter",
        CreatedAt = DateTime.Today
      },
      new User {
        Username = "user3",
        Email = "user3@test.com",
        Name = "User 3",
        Password = "user1234",
        Photo = "http://local.com/user3.jpg",
        Module = "Computer Science",
        Status = "Using Tryitter",
        CreatedAt = DateTime.Today
      },
      new User
      {
        Username = "user4",
        Email = "user4@test.com",
        Name = "User 4",
        Password = "user1234",
        Photo = "http://local.com/user4.jpg",
        Module = "Computer Science",
        Status = "Using Tryitter",
        CreatedAt = DateTime.Today
      }
    );

    modelBuilder.Entity<Post>().HasData(
      new Post {
        Id = Guid.NewGuid(),
        Text = "Post 1",
        Image = "http://local.com/post1.jpg",
        Username = "user1",
        CreatedAt = DateTime.Today,
        UpdatedAt = DateTime.Today
      },
      new Post {
        Id = Guid.NewGuid(),
        Text = "Post 2",
        Image = "http://local.com/post2.jpg",
        Username = "user1",
        CreatedAt = DateTime.Today,
        UpdatedAt = DateTime.Today
      },
      new Post {
        Id = Guid.NewGuid(),
        Text = "Post 3",
        Image = "http://local.com/post3.jpg",
        Username = "user1",
        CreatedAt = DateTime.Today,
        UpdatedAt = DateTime.Today
      },
      new Post
      {
        Id = Guid.NewGuid(),
        Text = "Post 1",
        Image = "http://local.com/post1.jpg",
        Username = "user2",
        CreatedAt = DateTime.Today,
        UpdatedAt = DateTime.Today
      },
      new Post
      {
        Id = Guid.NewGuid(),
        Text = "Post 2",
        Image = "http://local.com/post2.jpg",
        Username = "user2",
        CreatedAt = DateTime.Today,
        UpdatedAt = DateTime.Today
      },
      new Post
      {
        Id = Guid.NewGuid(),
        Text = "Post 1",
        Image = "http://local.com/post1.jpg",
        Username = "user3",
        CreatedAt = DateTime.Today,
        UpdatedAt = DateTime.Today
      }
    );
  }
}