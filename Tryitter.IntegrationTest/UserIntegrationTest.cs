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
                Username = "test0",
                Email = "test0@test.com",
                Name = "test 0",
                Password = "test1234",
                Photo = "http://local.com/test0.jpg",
                Module = "Computer Science",
                Status = "testing 0",
            },
            "{\"username\":\"test0\",\"email\":\"test0@test.com\",\"name\":\"test 0\""
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
            "test2",
            "{\"username\":\"test2\",\"email\":\"test2@test.com\",\"name\":\"test 2\""
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
                Username = "test3",
                Name = "test update",
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
            Username = "test3",
            Email = "test3@test.com",
            Name = "test 3",
            Password = "test1234",
            Photo = "http://local.com/test3.jpg",
            Module = "Computer Science",
            Status = "testing 3",
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

    public readonly static TheoryData<string, string> DeleteUserTestData =
    new()
    {
        {
            "/user",
            "test4"
        },
    };

    [Theory(DisplayName = "DELETE /User removes the user successfully")]
    [MemberData(nameof(DeleteUserTestData))]
    public async Task DeleteUserTest(string path, string usernameToDelete)
    {
        // Arrange
        var pathToDelete = $"{path}/{usernameToDelete}";
        User userToToken = new() {
            Username = "test4",
            Email = "test4@test.com",
            Name = "test 4",
            Password = "test1234",
            Photo = "http://local.com/test3.jpg",
            Module = "Computer Science",
            Status = "testing 4",
            CreatedAt = DateTime.Today
        };
        var token = new TokenGenerator().Generate(userToToken);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.DeleteAsync(pathToDelete);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}