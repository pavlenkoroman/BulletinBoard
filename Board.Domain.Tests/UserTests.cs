namespace Board.Domain.Tests;

using FluentAssertions;
using Xunit;

public sealed class UserTests
{
    [Theory]
    [InlineData("valid name", true)]
    public void CreateUser_ValidParameters_UserCreated(string name, bool isAdmin)
    {
        // Arrange & Act
        var user = User.Create(name, isAdmin);

        // Assertion
        user.Should().NotBeNull();
        user.Id.Should().NotBe(Guid.Empty);
        user.IsAdmin.Should().Be(isAdmin);
        user.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("22", true)]
    [InlineData(" ", false)]
    public void CreateUser_WithShortName_ThrowsArgumentOutOfRangeException(string name, bool isAdmin)
    {
        // Act
        Action action = () => User.Create(name, isAdmin);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CreateUser_WithTooLongName_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var tooLongName = new string('a', 101);

        // Act
        Action action = () => User.Create(tooLongName, true);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateUser_WithNullName_ThrowsArgumentException(string? name)
    {
        // Act
#pragma warning disable CS8604
        Action action = () => User.Create(name, false);
#pragma warning restore CS8604

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("first", "second")]
    public void UpdateUserName_Test(string initialName, string updatedName)
    {
        // Arrange
        var user = User.Create(initialName, true);

        // Act
        user.UpdateName(updatedName);

        // Assert
        user.Name.Should().Be(updatedName);
    }
}
