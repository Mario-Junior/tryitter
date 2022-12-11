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
using Tryitter.Repository;
using Tryitter.Auth;

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

    [Theory(DisplayName = "POST /User creates a new user successfully")]
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

    [Theory(DisplayName = "POST /User/login generates a JWT Token successfully")]
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

    public readonly static TheoryData<string, string, string> GetUserByUsernameTestData =
    new()
    {
        {
            "/user",
            "test1",
            "{\"username\":\"test1\",\"email\":\"test1@test.com\",\"name\":\"test 1\""
        },
    };

    [Theory(DisplayName = "GET /User/{username} returns an user by username successfully")]
    [MemberData(nameof(GetUserByUsernameTestData))]
    public async Task GetUserByUsernameTest(string path, string username, string responseJsonContent)
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

    [Theory(DisplayName = "GET /User returns an user list")]
    [InlineData("/user")]
    public async Task GetAllUsersTest(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public readonly static TheoryData<string, UserUpdateDTO, string> UpdateUserTestData =
    new()
    {
        {
            "/user",
            new UserUpdateDTO {
                Username = "test2",
                Name = "test Update",
                Status = "testing update",
            },
            "User updated"
        },
    };

    [Theory(DisplayName = "PUT /User updates the user successfully")]
    [MemberData(nameof(UpdateUserTestData))]
    public async Task UpdateUserTest(string path, UserUpdateDTO userToUpdate, string responseJsonContent)
    {
        // Arrange
        var userDataJson = JsonConvert.SerializeObject(userToUpdate);
        var requestContent = new StringContent(userDataJson, Encoding.UTF8, "application/json");
        User userToToken = new() {
            Username = "test2",
            Email = "test2@test.com",
            Name = "test 2",
            Password = "test1234",
            Photo = "http://local.com/test2.jpg",
            Module = "Computer Science",
            Status = "testing 2",
            CreatedAt = DateTime.Today
        };
        var token = new TokenGenerator().Generate(userToToken);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PutAsync(path, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().Contain(responseJsonContent);
    }
}