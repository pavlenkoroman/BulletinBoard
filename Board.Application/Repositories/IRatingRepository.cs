using Board.Domain;

namespace Board.Application.Repositories;

public interface IRatingRepository
{
    Task Add(Rating rating, Bulletin bulletin, CancellationToken cancellationToken);
    Task SwitchRatingType(Rating rating, Bulletin bulletin, CancellationToken cancellationToken);
    Task Remove(Rating rating, Bulletin bulletin, CancellationToken cancellationToken);
    Task<Rating?> TryGetRating(
        User user,
        Bulletin bulletin,
        CancellationToken cancellationToken);
}
