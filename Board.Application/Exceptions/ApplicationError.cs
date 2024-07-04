namespace Board.Application.Exceptions;

public sealed record ApplicationError
{
    public string Code { get; }
    public string? Description { get; init; }

    public ApplicationError(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        Code = code;
    }
}
