using System.Runtime.Serialization;

namespace Board.Contract.Responses;

[DataContract]
public sealed record ExceptionResponse
{
    public string Type { get; private init; }

    public string Message { get; private init; }

    public IEnumerable<string>? Details { get; init; }

    public string? StackTrace { get; init; }

    public ExceptionResponse(Type type, string message)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }

        Type = type.Name;
        Message = message;
    }
}
