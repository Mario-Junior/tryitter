using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tryitter.Application;
using Tryitter.Repository;

namespace Tryitter.IntegrationTest;
public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TryitterContext>));
      if (descriptor != null)
        services.Remove(descriptor);
      services.AddDbContext<TryitterContext>(options =>
      {
        options.UseInMemoryDatabase("InMemoryTest");
      });
      services.AddScoped<UserRepository>();
      services.AddScoped<PostRepository>();
      var sp = services.BuildServiceProvider();
      var scope = sp.CreateScope();
      var appContext = scope.ServiceProvider.GetRequiredService<TryitterContext>();
      appContext.Database.EnsureCreatedAsync();
      appContext.Database.EnsureDeletedAsync();
      appContext.Database.EnsureCreatedAsync();
      appContext.Users.AddRange(
          Helpers.GetUserListForTests()
      );
      appContext.Posts.AddRange(
          Helpers.GetPostListForTests()
      );
      appContext.SaveChanges();
    });
  }
}