using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Tryitter.Application;
using Tryitter.Models;
using Tryitter.DTO;
using Tryitter.Auth;

namespace Tryitter.IntegrationTest;

public class TryitterIntegrationTest : IClassFixture<TestingWebAppFactory<Program>>
{

    public HttpClient _client;

    public TryitterIntegrationTest(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // User entity tests //

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

    // Post entity tests //

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

    public readonly static TheoryData<string, string, Guid> DeletePostTestData =
    new()
    {
        {
            "/post",
            "test9",
            new Guid("123e4567e89b12d3a456426655440003")
        },
    };

    [Theory(DisplayName = "DELETE /Post/{username}/{postId} removes the post of the username successfully")]
    [MemberData(nameof(DeletePostTestData))]
    public async Task DeletePostTest(string path, string username, Guid postIdToDelete)
    {
        // Arrange
        var pathToDelete = $"{path}/{username}/{postIdToDelete}";
        User userToToken = new() {
            Username = "test9",
            Email = "test9@test.com",
            Name = "test delete post",
            Password = "test1234",
            Photo = "http://local.com/test9.jpg",
            Module = "Computer Science",
            Status = "testing 9",
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