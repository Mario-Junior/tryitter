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
}