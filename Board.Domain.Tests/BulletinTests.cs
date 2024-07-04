using Board.Domain.Tests.Data;
using FluentAssertions;
using Xunit;

namespace Board.Domain.Tests;

public class BulletinTests
{
    [Theory]
    [ClassData(typeof(BulletinValidData))]
    public void CreateBulletin_WithValidParameters_ShouldBeSuccess(
        int number,
        Guid userId,
        string text,
        Photo photo,
        DateTime expirationDate)
    {
        // Arrange & act
        var bulletin = Bulletin.Create(number, userId, text, photo, expirationDate);

        // Assertion
        bulletin.Should().NotBeNull();
        bulletin.Id.Should().NotBe(Guid.Empty);
        bulletin.Number.Should().Be(number);
        bulletin.UserId.Should().Be(userId);
        bulletin.Text.Should().Be(text);
        bulletin.Photo.Should().Be(photo);
        bulletin.Rating.Should().Be(0);
        bulletin.CreatedDate.Should().NotBe(default);
        bulletin.ExpirationDate.Should().Be(expirationDate);
    }

    [Theory]
    [ClassData(typeof(BulletinOutOfRangeData))]
    public void CreateBulletin_WithOutOfRangeParameters_ShouldThrow_ArgumentOutOfRangeException(
        Guid id,
        int number,
        Guid userId,
        string text,
        Photo photo,
        int rating,
        DateTime createdDate,
        DateTime expirationDate)
    {
        // Arrange & Act
        var action = () => new Bulletin(id, number, userId, text, photo, rating, createdDate, expirationDate);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [ClassData(typeof(BulletinValidData))]
    public void UpdateBulletinImage_WithValidText_ShouldBeSuccess(
        int number,
        Guid userId,
        string text,
        Photo photo,
        DateTime expirationDate)
    {
        // Arrange
        var bulletin = Bulletin.Create(number, userId, text, photo, expirationDate);
        var updateImage = new Photo(
            Guid.NewGuid(),
            "/a",
            new Uri("https://example2.com"),
            "/a_r",
            new Uri("https://resized2.com"));

        // Act
        bulletin.UpdatePhoto(updateImage);

        // Assert
        bulletin.Photo.Should().Be(updateImage);
    }

    [Theory]
    [ClassData(typeof(BulletinValidData))]
    public void UpdateBulletinText_WithValidText_ShouldBeSuccess(
        int number,
        Guid userId,
        string text,
        Photo photo,
        DateTime expirationDate)
    {
        // Arrange
        var bulletin = Bulletin.Create(number, userId, text, photo, expirationDate);

        var updateText = new string('a', 55);

        // Act
        bulletin.UpdateText(updateText);

        // Assert
        bulletin.Text.Should().Be(updateText);
    }
}
