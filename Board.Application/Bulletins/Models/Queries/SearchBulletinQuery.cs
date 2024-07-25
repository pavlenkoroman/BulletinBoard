using Board.Application.Models;
using Board.Domain;
using MediatR;

namespace Board.Application.Bulletins.Models.Queries;

public sealed record SearchBulletinQuery : BaseSearchRequest, IRequest<SearchResult<Bulletin>>
{
    public Range<int>? NumberRange { get; private init; }
    public Guid? UserId { get; private init; }
    public Range<int>? RatingRange { get; private init; }
    public Range<DateTime>? CreatedDateRange { get; private init; }
    public Range<DateTime>? ExpirationDateRange { get; private init; }
    public BulletinSortBy? SortBy { get; private init; }
    public SearchBulletinQuery(
        Page page,
        string? query,
        Range<int>? numberRange,
        Guid? userId,
        Range<int>? ratingRange,
        Range<DateTime>? createdDateRange,
        Range<DateTime>? expirationDateRange,
        BulletinSortBy? sortBy)
        : base(page, query)
    {
        NumberRange = numberRange;
        UserId = userId;
        RatingRange = ratingRange;
        CreatedDateRange = createdDateRange;
        ExpirationDateRange = expirationDateRange;
        SortBy = sortBy;
    }
}
