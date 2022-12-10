using Microsoft.EntityFrameworkCore;
using Tryitter.Models;

namespace Tryitter.Repository;

public class TryitterContext : DbContext
{
  public TryitterContext(DbContextOptions<TryitterContext> options) : base(options) {}

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<Post> Posts { get; set; } = null!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      var connectionString = "Server=127.0.0.1;Database=Tryitter;User=SA;Password=123@SQLServer;TrustServerCertificate=True";
      optionsBuilder.UseSqlServer(connectionString);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Post>()
      .HasOne(p => p.User)
      .WithMany(u => u.Posts)
      .HasForeignKey(p => p.Username);
  }
}