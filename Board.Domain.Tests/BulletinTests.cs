﻿using Board.Domain.Tests.Data;
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
        int expirationDays)
    {
        // Arrange & act
        var bulletin = Bulletin.Create(number, userId, text, photo, expirationDays);

        // Assertion
        bulletin.Should().NotBeNull();
        bulletin.Id.Should().NotBe(Guid.Empty);
        bulletin.Number.Should().Be(number);
        bulletin.UserId.Should().Be(userId);
        bulletin.Text.Should().Be(text);
        bulletin.Photo.Should().Be(photo);
        bulletin.Rating.Should().Be(0);
        bulletin.CreatedDate.Should().NotBe(default);
        bulletin.ExpirationDate.Year.Should().Be(bulletin.CreatedDate.AddDays(expirationDays).Year);
        bulletin.ExpirationDate.Month.Should().Be(bulletin.CreatedDate.AddDays(expirationDays).Month);
        bulletin.ExpirationDate.Day.Should().Be(bulletin.CreatedDate.AddDays(expirationDays).Day);
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
        var action = () => new Bulletin(
            id, 
            number, 
            userId, 
            text, 
            photo, 
            rating, 
            true, 
            createdDate, 
            expirationDate);

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
        int expirationDays)
    {
        // Arrange
        var bulletin = Bulletin.Create(number, userId, text, photo, expirationDays);
        var updateImage = new Photo(
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
        int expirationDays)
    {
        // Arrange
        var bulletin = Bulletin.Create(number, userId, text, photo, expirationDays);

        var updateText = new string('a', 55);

        // Act
        bulletin.UpdateText(updateText);

        // Assert
        bulletin.Text.Should().Be(updateText);
    }
}
