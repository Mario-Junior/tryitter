using Microsoft.EntityFrameworkCore;
using Tryitter.Auth;
using Tryitter.Models;
using Tryitter.Repository;

namespace Tryitter.UnitTest;

public class UserRepositoryTest
{
    public readonly static TheoryData<TryitterContext, UserCreateDTO, string, string> CreateUserTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("CreateUserTest"),
            new UserCreateDTO {
                Username = "test4",
                Email = "test4@test.com",
                Name = "test 4",
                Password = "test1234",
                Photo = "http://local.com/test4.jpg",
                Module = "Computer Science",
                Status = "testing 4",
            },
            "test4",
            "test4@test.com"
        },
    };

    [Theory(DisplayName = "Create a new user successfully")]
    [MemberData(nameof(CreateUserTestData))]
    public async Task CreateUserTest(TryitterContext context, UserCreateDTO newUser, string usernameExpected, string emailExpected)
    {
        context.ChangeTracker.Clear();

        UserRepository _userRepository = new(context);
        var result = await _userRepository.CreateUser(newUser);

        result.Username.Should().Be(usernameExpected);
        result.Email.Should().Be(emailExpected);
    }

    public readonly static TheoryData<TryitterContext, string, string, User> LoginValidateOkTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("LoginValidateOkTest"),
            "test1",
            "test1234",
            new User{
                    Username = "test1",
                    Email = "test1@test.com",
                    Name = "test 1",
                    Password = "test1234",
                    Photo = "http://local.com/test1.jpg",
                    Module = "Computer Science",
                    Status = "testing 1",
                    CreatedAt = DateTime.Today
                }
        },
    };

    [Theory(DisplayName = "Login user successfully")]
    [MemberData(nameof(LoginValidateOkTestData))]
    public async Task LoginValidateOkTest(TryitterContext context, string username, string password, User userFound)
    {
        context.ChangeTracker.Clear();

        UserRepository _userRepository = new(context);
        var result = await _userRepository.LoginValidate(username, password);

        result.Should().BeEquivalentTo(userFound);
    }

    public readonly static TheoryData<TryitterContext, string, string, User> LoginValidateFailTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("LoginValidateFailTest"),
            "test1",
            "test123456",
            new User{
                    Username = "test1",
                    Email = "test1@test.com",
                    Name = "test 1",
                    Password = "test1234",
                    Photo = "http://local.com/test1.jpg",
                    Module = "Computer Science",
                    Status = "testing 1",
                    CreatedAt = DateTime.Today
                }
        },
    };

    [Theory(DisplayName = "Login user not successfully")]
    [MemberData(nameof(LoginValidateFailTestData))]
    public async Task LoginValidateFailTest(TryitterContext context, string username, string passwordWrong, User userFound)
    {
        context.ChangeTracker.Clear();

        UserRepository _userRepository = new(context);
        var result = await _userRepository.LoginValidate(username, passwordWrong);

        result.Should().NotBeEquivalentTo(userFound);
        result.Should().BeNull();
    }
}