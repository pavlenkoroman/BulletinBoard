using Board.Domain;

namespace Board.Application.Exceptions;

public class AlreadyExistsException : ApplicationException
{
    private AlreadyExistsException(ApplicationError error) : base(error)
    {
    }

    public static AlreadyExistsException CreateByExistingRating(Guid userId, Guid bulletinId, RatingType ratingType)
    {
        return new AlreadyExistsException(new ApplicationError(ApplicationErrorCode.RatingAlreadyExists)
        {
            Description = $"User {userId} already added rating {bulletinId} with type ({ratingType})"
        });
    }
}
