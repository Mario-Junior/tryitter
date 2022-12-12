using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Tryitter.Application;
using Tryitter.Models;
using Tryitter.DTO;
using Tryitter.Repository;
using Tryitter.Auth;

namespace Tryitter.IntegrationTest;

public class PostIntegrationTest : IClassFixture<TestingWebAppFactory<Program>>
{

    public HttpClient _client;

    public PostIntegrationTest(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    public readonly static TheoryData<string, PostCreateDTO, string> CreatePostTestData =
    new()
    {
        {
            "/post",
            new PostCreateDTO {
                Text = "Post 1",
                Image = "http://local.com/post1.jpg",
                Username = "test5",
            },
            "\"text\":\"Post 1\",\"image\":\"http://local.com/post1.jpg\",\"username\":\"test5\""
        },
    };

    [Theory(DisplayName = "POST /Post creates a new post successfully")]
    [MemberData(nameof(CreatePostTestData))]
    public async Task CreatePostTest(string path, PostCreateDTO newUser, string responseJsonContent)
    {
        // Arrange
        var userDataJson = JsonConvert.SerializeObject(newUser);
        var requestContent = new StringContent(userDataJson, Encoding.UTF8, "application/json");
        User userToToken = new() {
            Username = "test5",
            Email = "test5@test.com",
            Name = "test create post",
            Password = "test1234",
            Photo = "http://local.com/test5.jpg",
            Module = "Computer Science",
            Status = "testing 5",
            CreatedAt = DateTime.Today
        };
        var token = new TokenGenerator().Generate(userToToken);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PostAsync(path, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseContent.Should().Contain(responseJsonContent);
    }

    public readonly static TheoryData<string, string, string> GetPostsByUsernameTestData =
    new()
    {
        {
            "/post",
            "test6",
            "\"text\":\"Post 1\",\"image\":\"http://local.com/post1.jpg\",\"username\":\"test6\""
        },
    };

    [Theory(DisplayName = "GET /Post/{username} returns a post list by username successfully")]
    [MemberData(nameof(GetPostsByUsernameTestData))]
    public async Task GetPostsByUsernameTest(string path, string username, string responseJsonContent)
    {
        // Arrange
        var pathToGet = $"{path}/{username}";

        // Act
        var response = await _client.GetAsync(pathToGet);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().Contain(responseJsonContent);
    }

    public readonly static TheoryData<string, string, Guid, string> GetPostsByUsernameAndIdTestData =
    new()
    {
        {
            "/post",
            "test7",
            new Guid("123e4567e89b12d3a456426655440001"),
            "\"text\":\"Post 1\",\"image\":\"http://local.com/post1.jpg\",\"username\":\"test7\""
        },
    };

    [Theory(DisplayName = "GET /Post/{username}/{postId} returns a post by username and postId successfully")]
    [MemberData(nameof(GetPostsByUsernameAndIdTestData))]
    public async Task GetPostsByUsernameAndIdTest(string path, string username, Guid postId, string responseJsonContent)
    {
        // Arrange
        var pathToGet = $"{path}/{username}/{postId}";

        // Act
        var response = await _client.GetAsync(pathToGet);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().Contain(responseJsonContent);
    }

    [Theory(DisplayName = "GET /Post returns a post list")]
    [InlineData("/post")]
    public async Task GetAllPostsTest(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public readonly static TheoryData<string, PostUpdateDTO, string, string> UpdatePostTestData =
    new()
    {
        {
            "/post",
            new PostUpdateDTO {
                Id = new Guid("123e4567e89b12d3a456426655440002"),
                Text = "Post update",
            },
            "test8",
            "Post updated"
        },
    };

    [Theory(DisplayName = "PUT /Post updates the post successfully")]
    [MemberData(nameof(UpdatePostTestData))]
    public async Task UpdatePostTest(string path, PostUpdateDTO postToUpdate, string username, string responseJsonContent)
    {
        // Arrange
        var pathToPut = $"{path}/{username}";
        var postDataJson = JsonConvert.SerializeObject(postToUpdate);
        var requestContent = new StringContent(postDataJson, Encoding.UTF8, "application/json");
        User userToToken = new() {
            Username = "test8",
            Email = "test8@test.com",
            Name = "test update post",
            Password = "test1234",
            Photo = "http://local.com/test8.jpg",
            Module = "Computer Science",
            Status = "testing 8",
            CreatedAt = DateTime.Today
        };
        var token = new TokenGenerator().Generate(userToToken);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PutAsync(pathToPut, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().Contain(responseJsonContent);
    }
}