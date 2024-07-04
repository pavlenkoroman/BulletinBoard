namespace Board.Application.Exceptions;

public class LimitException : ApplicationException
{
    private LimitException(ApplicationError error) : base(error)
    {
    }
    
    public static LimitException CreateByUserId(Guid userId)
    {
        return new LimitException(new ApplicationError(ApplicationErrorCode.BulletinsLimitExceeded)
        {
            Description = $"{userId}"
        });
    }
}
