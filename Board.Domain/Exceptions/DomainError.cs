namespace Board.Domain.Exceptions;

public record DomainError
{
        public string Code { get; }
        public string? Description { get; init; }

        public DomainError(string code)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);

            Code = code;
        }
}
