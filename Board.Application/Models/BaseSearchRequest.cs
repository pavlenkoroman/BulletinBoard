namespace Board.Application.Models;

public record BaseSearchRequest
{
    public Page Page { get; }
    public string? Query { get; }

    public BaseSearchRequest(Page page, string? query)
    {
        ArgumentNullException.ThrowIfNull(page);

        Page = page;
        Query = query;
    }
}
