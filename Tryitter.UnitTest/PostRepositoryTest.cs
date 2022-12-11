using Microsoft.EntityFrameworkCore;
using Tryitter.Models;
using Tryitter.Repository;

namespace Tryitter.UnitTest;

public class PostRepositoryTest
{
    public readonly static TheoryData<TryitterContext, PostCreateDTO, string, string> CreatePostTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("CreatePostTest"),
            new PostCreateDTO {
                Text = "Post 5",
                Image = "http://local.com/post1.jpg",
                Username = "test1",
            },
            "test1",
            "Post 5"
        },
    };

    [Theory(DisplayName = "Create a new post successfully")]
    [MemberData(nameof(CreatePostTestData))]
    public async Task CreatePostTest(TryitterContext context, PostCreateDTO newPost, string usernameExpected, string textExpected)
    {
        // Arrange
        context.ChangeTracker.Clear();
        PostRepository _postRepository = new(context);

        // Act
        var result = await _postRepository.CreatePost(newPost);

        // Assert
        result.Username.Should().Be(usernameExpected);
        result.Text.Should().Be(textExpected);
    }

    public readonly static TheoryData<TryitterContext, string, int> GetPostsByUsernameTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("GetPostsByUsernameTest"),
            "test1",
            4
        },
    };

    [Theory(DisplayName = "Get posts by username successfully")]
    [MemberData(nameof(GetPostsByUsernameTestData))]
    public async Task GetPostsByUsernameTest(TryitterContext context, string username, int postListLength)
    {
        // Arrange
        context.ChangeTracker.Clear();
        PostRepository _postRepository = new(context);

        // Act
        var result = await _postRepository.GetPostsByUsername(username);

        // Assert
        result.Count().Should().Be(postListLength);
    }

    public readonly static TheoryData<TryitterContext, string, Guid, PostGetDTO> GetPostByUsernameAndIdOkTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("GetPostByUsernameAndIdOkTest"),
            "test1",
            new Guid("123e4567e89b12d3a456426655440000"),
            new PostGetDTO {
                Id = new Guid("123e4567e89b12d3a456426655440000"),
                Text = "Post 1",
                Image = "http://local.com/post1.jpg",
                Username = "test1",
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            }
        },
    };

    [Theory(DisplayName = "Get post by username and Id successfully")]
    [MemberData(nameof(GetPostByUsernameAndIdOkTestData))]
    public async Task GetPostByUsernameAndIdOkTest(TryitterContext context, string username, Guid postId, PostGetDTO postFound)
    {
        // Arrange
        context.ChangeTracker.Clear();
        PostRepository _postRepository = new(context);

        // Act
        var result = await _postRepository.GetPostByUsernameAndId(username, postId);

        // Assert
        result.Should().BeEquivalentTo(postFound);
    }

    public readonly static TheoryData<TryitterContext, string, Guid, PostGetDTO> GetPostByUsernameAndIdFailTestData =
    new()
    {
        {
            Helpers.GetContextInstanceForTests("GetPostByUsernameAndIdFailTest"),
            "test1",
            new Guid("123e4567e89b12d3a456426655440001"),
            new PostGetDTO {
                Id = new Guid("123e4567e89b12d3a456426655440000"),
                Text = "Post 1",
                Image = "http://local.com/post1.jpg",
                Username = "test1",
                CreatedAt = DateTime.Today,
                UpdatedAt = DateTime.Today
            }
        },
    };

    [Theory(DisplayName = "Get post by username and Id not successfully")]
    [MemberData(nameof(GetPostByUsernameAndIdFailTestData))]
    public async Task GetPostByUsernameAndIdFailTest(TryitterContext context, string username, Guid postIdWrong, PostGetDTO postExpected)
    {
        // Arrange
        context.ChangeTracker.Clear();
        PostRepository _postRepository = new(context);

        // Act
        var result = await _postRepository.GetPostByUsernameAndId(username, postIdWrong);

        // Assert
        result.Should().NotBeEquivalentTo(postExpected);
        result.Should().BeNull();
    }
}