namespace Board.Application.Models.Search;

public class SearchResult<T>
{
    public int CurrentPage { get; }
    public int PageSize { get; }
    public IReadOnlyCollection<T> Results { get; }

    public SearchResult(int currentPage, int pageSize, IReadOnlyCollection<T> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        CurrentPage = currentPage;
        PageSize = pageSize;
        Results = results;
    }
}
