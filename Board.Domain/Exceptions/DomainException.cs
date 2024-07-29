namespace Board.Domain.Exceptions;

public class DomainException : Exception
{
    public IEnumerable<DomainError>? Errors { get; protected set; }

    public DomainException(DomainError error)
    {
        Errors = new[] { error };
    }
}
