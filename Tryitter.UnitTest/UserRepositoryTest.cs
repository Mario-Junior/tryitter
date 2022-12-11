using Microsoft.EntityFrameworkCore;
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
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.CreateUser(newUser);

        // Assert
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
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.LoginValidate(username, password);

        // Assert
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
        // Arange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.LoginValidate(username, passwordWrong);

        // Assert
        result.Should().NotBeEquivalentTo(userFound);
        result.Should().BeNull();
    }

    public readonly static TheoryData<TryitterContext, string, UserGetDTO> GetUserByUsernameOkData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("GetUserByUsernameOkTest"),
            "test3",
            new UserGetDTO {
                    Username = "test3",
                    Email = "test3@test.com",
                    Name = "test 3",
                    Photo = "http://local.com/test3.jpg",
                    Module = "Computer Science",
                    Status = "testing 3",
                    CreatedAt = DateTime.Today,
                    Posts = new List<PostGetDTO> {},
                }
        },
    };

    [Theory(DisplayName = "Get user by username successfully")]
    [MemberData(nameof(GetUserByUsernameOkData))]
    public async Task GetUserByUsernameOkTest(TryitterContext context, string username, UserGetDTO userFound)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.GetUserByUsername(username);

        // Assert
        result.Should().BeEquivalentTo(userFound);
    }

    public readonly static TheoryData<TryitterContext, string, UserGetDTO> GetUserByUsernameFailData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("GetUserByUsernameFailTest"),
            "test4",
            new UserGetDTO {
                    Username = "test3",
                    Email = "test3@test.com",
                    Name = "test 3",
                    Photo = "http://local.com/test3.jpg",
                    Module = "Computer Science",
                    Status = "testing 3",
                    CreatedAt = DateTime.Today,
                    Posts = new List<PostGetDTO> {},
                }
        },
    };

    [Theory(DisplayName = "Get user by username not successfully")]
    [MemberData(nameof(GetUserByUsernameFailData))]
    public async Task GetUserByUsernameFailTest(TryitterContext context, string usernameWrong, UserGetDTO userFound)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.GetUserByUsername(usernameWrong);

        // Assert
        result.Should().NotBeEquivalentTo(userFound);
        result.Should().BeNull();
    }

    public readonly static TheoryData<TryitterContext, int> GetAllUsersData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("GetAllUsersTest"),
            3
        },
    };

    [Theory(DisplayName = "Get all users successfully")]
    [MemberData(nameof(GetAllUsersData))]
    public async Task GetAllUsersTest(TryitterContext context, int userListLength)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.GetAllUsers();

        // Assert
        result.Count().Should().Be(userListLength);
    }

    public readonly static TheoryData<TryitterContext, UserUpdateDTO> UpdateUserOkTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("UpdateUserOkTest"),
            new UserUpdateDTO {
                    Username = "test1",
                    Name = "test update",
                    Status = "testing update"
                }
        },
    };

    [Theory(DisplayName = "Update user successfully")]
    [MemberData(nameof(UpdateUserOkTestData))]
    public async Task UpdateUserOkTest(TryitterContext context, UserUpdateDTO userToUpdate)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.UpdateUser(userToUpdate);

        // Assert
        result.Should().BeTrue();
    }

    public readonly static TheoryData<TryitterContext, UserUpdateDTO> UpdateUserFailTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("UpdateUserFailTest"),
            new UserUpdateDTO {
                    Username = "test100",
                    Name = "test update",
                    Status = "testing update"
                }
        },
    };

    [Theory(DisplayName = "Update user not successfully")]
    [MemberData(nameof(UpdateUserFailTestData))]
    public async Task UpdateUserFailTest(TryitterContext context, UserUpdateDTO userToUpdateWrong)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.UpdateUser(userToUpdateWrong);

        // Assert
        result.Should().BeFalse();
    }

    public readonly static TheoryData<TryitterContext, string> DeleteUserOkTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("DeleteUserOkTest"),
            "test3"
        },
    };

    [Theory(DisplayName = "Delete user successfully")]
    [MemberData(nameof(DeleteUserOkTestData))]
    public async Task DeleteUserOkTest(TryitterContext context, string usernameToDelete)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.DeleteUser(usernameToDelete);

        // Assert
        result.Should().BeTrue();
    }

    public readonly static TheoryData<TryitterContext, string> DeleteUserFailTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("DeleteUserFailTest"),
            "test100"
        },
    };

    [Theory(DisplayName = "Delete user not successfully")]
    [MemberData(nameof(DeleteUserFailTestData))]
    public async Task DeleteUserFailTest(TryitterContext context, string usernameToDeleteWrong)
    {
        // Arrange
        context.ChangeTracker.Clear();
        UserRepository _userRepository = new(context);

        // Act
        var result = await _userRepository.DeleteUser(usernameToDeleteWrong);

        // Assert
        result.Should().BeFalse();
    }
}