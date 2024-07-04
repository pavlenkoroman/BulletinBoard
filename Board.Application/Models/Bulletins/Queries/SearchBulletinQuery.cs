using Board.Application.Models.Search;
using Board.Domain;
using MediatR;

namespace Board.Application.Models.Bulletins.Queries;

public sealed record SearchBulletinQuery : BaseSearchRequest, IRequest<SearchResult<Bulletin>>
{
    public IntegerRange? NumberRange { get; private init; }
    public Guid? UserId { get; private init; }
    public IntegerRange? RatingRange { get; private init; }
    public DateTimeRange? CreatedDateRange { get; private init; }
    public DateTimeRange? ExpirationDateRange { get; private init; }

    public SearchBulletinQuery(
        Page page,
        string? query,
        IntegerRange? numberRange,
        Guid? userId,
        IntegerRange? ratingRange,
        DateTimeRange? createdDateRange,
        DateTimeRange? expirationDateRange)
        : base(page, query)
    {
        NumberRange = numberRange;
        UserId = userId;
        RatingRange = ratingRange;
        CreatedDateRange = createdDateRange;
        ExpirationDateRange = expirationDateRange;
    }
}
