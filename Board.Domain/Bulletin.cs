namespace Board.Domain;

public sealed class Bulletin
{
    public Guid Id { get; private init; }
    public int Number { get; private init; }
    public Guid UserId { get; private init; }
    public string Text { get; private set; }
    public Uri Image { get; private set; }
    public int Rating { get; private set; }
    public DateTime CreatedDate { get; private init; }
    public DateTime ExpirationDate { get; private init; }

    public Bulletin(
        Guid id,
        int number,
        Guid userId,
        string text,
        Uri image,
        int rating,
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
        Image = image;
        Rating = rating;
        CreatedDate = createdDate;
        ExpirationDate = expirationDate;
    }

    public static Bulletin Create(int number, Guid userId, string text, Uri image, DateTime expirationDate)
    {
        return new Bulletin(
            Guid.NewGuid(),
            number,
            userId,
            text,
            image,
            0,
            DateTime.UtcNow,
            expirationDate);
    }

    public void UpdateText(string text)
    {
        ValidateText(text);
        Text = text;
    }

    public void UpdateImage(Uri image)
    {
        Image = image;
    }

    private void ValidateText(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        ArgumentOutOfRangeException.ThrowIfLessThan(text.Length, 50);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 1000);
    }
}
