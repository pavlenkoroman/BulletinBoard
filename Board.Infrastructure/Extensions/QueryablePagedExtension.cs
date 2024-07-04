using Board.Application.Models.Search;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Extensions;

public static class QueryablePagedExtension
{
    public static async Task<SearchResult<T>> GetPagedAsync<T>(
        this IQueryable<T> query,
        Page page,
        CancellationToken cancellationToken = default)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(page);

        query = query
            .Skip(page.PageNumber * page.PageSize)
            .Take(page.PageSize);

        var results = await query
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new SearchResult<T>(
            page.PageNumber,
            page.PageSize,
            results);
    }
}
