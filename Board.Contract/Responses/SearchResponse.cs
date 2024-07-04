using System.Runtime.Serialization;

namespace Board.Contract.Responses;

[DataContract]
public sealed record SearchResponse<T>
{
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyCollection<T> Results { get; init; } = null!;
}
