namespace Board.Domain;

public sealed class Bulletin
{
    public Guid Id { get; private init; }
    public int Number { get; private init; }
    public Guid UserId { get; private init; }
    public string Text { get; private set; }
    public Photo Photo { get; private set; }
    public int Rating { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedDate { get; private init; }
    public DateTime ExpirationDate { get; private init; }

    public Bulletin(
        Guid id,
        int number,
        Guid userId,
        string text,
        Photo photo,
        int rating,
        bool isActive,
        DateTime createdDate,
        DateTime expirationDate)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, default);
        ArgumentOutOfRangeException.ThrowIfEqual(userId, default);
        ValidateText(text);
        ArgumentOutOfRangeException.ThrowIfEqual(createdDate, default);
        ArgumentOutOfRangeException.ThrowIfEqual(expirationDate, default);

        if (expirationDate <= createdDate)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expirationDate),
                "Expiration date cannot be less or equal created date");
        }

        Id = id;
        Number = number;
        UserId = userId;
        Text = text;
        Photo = photo;
        Rating = rating;
        IsActive = isActive;
        CreatedDate = createdDate;
        ExpirationDate = expirationDate;
    }

    private Bulletin()
    {
    }

    public static Bulletin Create(int number, Guid userId, string text, Photo photo, int expirationDays)
    {
        return new Bulletin(
            Guid.NewGuid(),
            number,
            userId,
            text,
            photo,
            0,
            true,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(expirationDays));
    }

    public void UpdateText(string text)
    {
        ValidateText(text);
        Text = text;
    }

    public void UpdatePhoto(Photo photo)
    {
        Photo = photo;
    }

    public void UpdateIsActive(bool isActive)
    {
        IsActive = isActive;
    }

    private void ValidateText(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        ArgumentOutOfRangeException.ThrowIfLessThan(text.Length, 50);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 1000);
    }

    public void UpdateRating(int points)
    {
        Rating += points;
    }
}
