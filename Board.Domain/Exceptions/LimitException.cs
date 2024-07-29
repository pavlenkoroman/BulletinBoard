namespace Board.Domain.Exceptions;

public class LimitException : DomainException
{
    private LimitException(DomainError error) : base(error)
    {
    }
    
    public static LimitException CreateByUserId(Guid userId)
    {
        return new LimitException(new DomainError(DomainErrorCode.BulletinsLimitExceeded)
        {
            Description = $"{userId}"
        });
    }
}
