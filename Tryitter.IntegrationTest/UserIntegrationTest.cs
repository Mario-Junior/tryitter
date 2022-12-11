using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tryitter.Application;
using Tryitter.Models;
using Tryitter.Repository;
using System.Text;

namespace Tryitter.IntegrationTest;

public class UserIntegrationTest : IClassFixture<TestingWebAppFactory<Program>>
{

    public HttpClient _client;

    public UserIntegrationTest(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    public readonly static TheoryData<string, UserCreateDTO, string> CreateUserTestData =
    new()
    {
        {
            "/user",
            new UserCreateDTO {
                Username = "test4",
                Email = "test4@test.com",
                Name = "test 4",
                Password = "test1234",
                Photo = "http://local.com/test4.jpg",
                Module = "Computer Science",
                Status = "testing 4",
            },
            "{\"username\":\"test4\",\"email\":\"test4@test.com\",\"name\":\"test 4\""
        },
    };

    [Theory(DisplayName = "POST /User creates new user")]
    [MemberData(nameof(CreateUserTestData))]
    public async Task CreateUserTest(string path, UserCreateDTO newUser, string responseJsonContent)
    {
        // Arrange
        var userDataJson = JsonConvert.SerializeObject(newUser);
        var requestContent = new StringContent(userDataJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(path, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseContent.Should().Contain(responseJsonContent);
    }

    public readonly static TheoryData<string, UserLoginDTO, string> LoginTestData =
    new()
    {
        {
            "/user/login",
            new UserLoginDTO {
                Username = "test1",
                Password = "test1234",
            },
            "Bearer "
        },
    };

    [Theory(DisplayName = "POST /User/login generates JWT Token successfully")]
    [MemberData(nameof(LoginTestData))]
    public async Task LoginTest(string path, UserLoginDTO userToLogin, string responseJsonContent)
    {
        // Arrange
        var userDataJson = JsonConvert.SerializeObject(userToLogin);
        var requestContent = new StringContent(userDataJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(path, requestContent);
        var responseTokenContent = await response.Content.ReadAsStringAsync();
        var tokenParts = responseTokenContent.Split(".");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseTokenContent.Should().Contain(responseJsonContent);
        tokenParts.Length.Should().Be(3);
    }

    [Theory(DisplayName = "GET /User returns an user list")]
    [InlineData("/user")]
    public async Task GetAllUsersTest(string url)
    {
        var response = await _client.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}