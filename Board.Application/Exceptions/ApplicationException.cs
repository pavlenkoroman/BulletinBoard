namespace Board.Application.Exceptions;

public class ApplicationException : Exception
{
    public IEnumerable<ApplicationError>? Errors { get; protected set; }

    public ApplicationException(ApplicationError error)
    {
        Errors = new[] { error };
    }
}
