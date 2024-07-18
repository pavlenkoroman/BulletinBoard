using System.Runtime.Serialization;

namespace Board.Contract.Responses;

[DataContract]
public sealed record SearchResponse<T>
{
    public IReadOnlyCollection<T> Results { get; init; } = null!;
    public int Count { get; init; }
}
