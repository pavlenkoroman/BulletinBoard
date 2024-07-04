using Board.Application.Repositories;
using Board.Domain;
using Board.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly DataContext _dbContext;

    public RatingRepository(DataContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task Add(Rating rating, Bulletin bulletin, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rating);

        bulletin.UpdateRating(
            rating.RatingType switch
            {
                RatingType.Increase => 1,
                RatingType.Decrease => -1,
                _ => throw new ArgumentOutOfRangeException()
            });

        await _dbContext.AddAsync(rating, cancellationToken);
    }

    public Task SwitchRatingType(Rating rating, Bulletin bulletin, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bulletin);
        ArgumentNullException.ThrowIfNull(rating);

        switch (rating.RatingType)
        {
            case RatingType.Increase:
                rating.UpdateRatingType(RatingType.Decrease);
                bulletin.UpdateRating(-2);
                break;
            case RatingType.Decrease:
                rating.UpdateRatingType(RatingType.Increase);
                bulletin.UpdateRating(2);
                break;
        }

        return Task.CompletedTask;
    }

    public Task Remove(Rating rating, Bulletin bulletin, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(rating);
        ArgumentNullException.ThrowIfNull(bulletin);

        _dbContext.Remove(rating);

        bulletin.UpdateRating(rating.RatingType switch
        {
            RatingType.Increase => -1,
            RatingType.Decrease => 1,
            _ => throw new ArgumentOutOfRangeException()
        });

        return Task.CompletedTask;
    }

    public async Task<Rating?> TryGetRating(User user, Bulletin bulletin, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Rating>().SingleOrDefaultAsync(
            rating => rating.UserId == user.Id && rating.BulletinId == bulletin.Id, cancellationToken);
    }
}
