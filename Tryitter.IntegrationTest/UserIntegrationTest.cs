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

    [Theory(DisplayName = "GET /User returns an user list")]
    [InlineData("/user")]
    public async Task GetAllUsersTest(string url)
    {
        var response = await _client.GetAsync(url);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}