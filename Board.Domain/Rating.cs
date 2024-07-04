namespace Board.Domain;

public sealed class Rating
{
    public Guid UserId { get; private init; }
    public Guid BulletinId { get; private init; }
    public RatingType RatingType { get; private set; }

    public Rating(Guid userId, Guid bulletinId, RatingType ratingType)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(userId, default);
        ArgumentOutOfRangeException.ThrowIfEqual(bulletinId, default);

        UserId = userId;
        BulletinId = bulletinId;
        RatingType = ratingType;
    }

    public static Rating Create(User user, Bulletin bulletin, RatingType ratingType)
    {
        return new Rating(user.Id, bulletin.Id, ratingType);
    }

    public void UpdateRatingType(RatingType ratingType)
    {
        RatingType = ratingType;
    }
}
