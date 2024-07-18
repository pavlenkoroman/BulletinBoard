namespace Board.Application.Models.Search;

public class SearchResult<T>
{
    public IReadOnlyCollection<T> Results { get; }
    public int Count { get; }


    public SearchResult(IReadOnlyCollection<T> results, int count)
    {
        ArgumentNullException.ThrowIfNull(results);

        Results = results;
        Count = count;
    }
}
