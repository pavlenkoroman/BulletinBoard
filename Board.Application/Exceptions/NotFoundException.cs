namespace Board.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    private NotFoundException(ApplicationError error) : base(error)
    {
    }

    public static NotFoundException CreateByUserId(Guid userId)
    {
        return new NotFoundException(new ApplicationError(ApplicationErrorCode.UserNotFound)
        {
            Description = userId.ToString()
        });
    }
    
    public static NotFoundException CreateByBulletinId(Guid bulletinId)
    {
        return new NotFoundException(new ApplicationError(ApplicationErrorCode.BulletinNotFound)
        {
            Description = bulletinId.ToString()
        });
    }
    
    public static NotFoundException CreateForRating(Guid userId, Guid bulletinId)
    {
        return new NotFoundException(new ApplicationError(ApplicationErrorCode.RatingNotFound)
        {
            Description = $"{userId} {bulletinId}"
        });
    }
}
