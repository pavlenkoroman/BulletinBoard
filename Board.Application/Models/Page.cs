namespace Board.Application.Models;

public class Page
{
    public int PageNumber { get; private init; }
    public int PageSize { get; private init; }

    public Page(int pageNumber, int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pageNumber, nameof(pageNumber));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize, nameof(pageSize));

        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
