using Board.Application.Exceptions;
using Board.Application.Models.Bulletins;
using Board.Application.Models.Bulletins.Queries;
using Board.Application.Models.Search;
using Board.Application.Repositories;
using Board.Domain;
using Board.Infrastructure.Context;
using Board.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Repositories;

public class BulletinRepository : IBulletinRepository
{
    private readonly DataContext _dbContext;

    public BulletinRepository(DataContext dbContext, IRatingRepository rating)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(rating);

        _dbContext = dbContext;
        Rating = rating;
    }

    public IRatingRepository Rating { get; }

    public async Task Create(Bulletin bulletin, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bulletin);

        await _dbContext.Set<Bulletin>().AddAsync(bulletin, cancellationToken);
    }

    public async Task<Bulletin> GetById(Guid bulletinId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Bulletin>()
                   .SingleOrDefaultAsync(bulletin => bulletin.Id == bulletinId, cancellationToken)
               ?? throw NotFoundException.CreateByBulletinId(bulletinId);
    }

    public async Task<Bulletin> GetByUserId(
        Guid currentUserId,
        Guid bulletinId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Bulletin>()
                   .SingleOrDefaultAsync(bulletin =>
                       bulletin.UserId == currentUserId && bulletin.Id == bulletinId, cancellationToken)
               ?? throw NotFoundException.CreateByBulletinId(bulletinId);
    }

    public async Task<int> GetCountByUserId(Guid currentUserId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Bulletin>()
            .CountAsync(bulletin => bulletin.UserId == currentUserId, cancellationToken);
    }

    public void Delete(Bulletin bulletin)
    {
        ArgumentNullException.ThrowIfNull(bulletin);

        _dbContext.Set<Bulletin>().Remove(bulletin);
    }

    public async Task<SearchResult<Bulletin>> Search(SearchBulletinQuery query, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Set<Bulletin>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Query))
        {
            queryable = queryable.Where(bulletin => EF.Functions.ILike(bulletin.Text, $"%{query.Query}%"));
        }

        if (query.UserId is not null)
        {
            queryable = queryable.Where(bulletin => bulletin.UserId == query.UserId);
        }

        if (query.NumberRange is not null)
        {
            if (query.NumberRange.Start is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.Number >= query.NumberRange.Start);
            }

            if (query.NumberRange.End is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.Number <= query.NumberRange.End);
            }
        }

        if (query.RatingRange is not null)
        {
            if (query.RatingRange.Start is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.Rating >= query.RatingRange.Start);
            }

            if (query.RatingRange.End is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.Rating <= query.RatingRange.End);
            }
        }

        if (query.CreatedDateRange is not null)
        {
            if (query.CreatedDateRange.Start is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.CreatedDate >= query.CreatedDateRange.Start);
            }

            if (query.CreatedDateRange.End is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.CreatedDate <= query.CreatedDateRange.End);
            }
        }

        if (query.ExpirationDateRange is not null)
        {
            if (query.ExpirationDateRange.Start is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.CreatedDate >= query.ExpirationDateRange.Start);
            }

            if (query.ExpirationDateRange.End is not null)
            {
                queryable = queryable.Where(bulletin => bulletin.CreatedDate <= query.ExpirationDateRange.End);
            }
        }

        queryable = SortBulletins(queryable, query.SortBy);

        var bulletinsCounter = await queryable.CountAsync(cancellationToken);

        return await queryable.GetPagedAsync(query.Page, bulletinsCounter, cancellationToken);
    }


    private static IQueryable<Bulletin> SortBulletins(IQueryable<Bulletin> bulletins, BulletinSortBy? sortBy)
    {
        return sortBy switch
        {
            BulletinSortBy.Number => bulletins.OrderBy(bulletin => bulletin.Number),
            BulletinSortBy.Text => bulletins.OrderBy(bulletin => bulletin.Text),
            BulletinSortBy.Rating => bulletins.OrderBy(bulletin => bulletin.Rating),
            BulletinSortBy.CreatedDateAscending => bulletins.OrderBy(bulletin => bulletin.CreatedDate),
            BulletinSortBy.ExpirationDateAscending => bulletins.OrderBy(bulletin => bulletin.ExpirationDate),
            BulletinSortBy.CreatedDateDescending => bulletins.OrderByDescending(bulletin => bulletin.CreatedDate),
            BulletinSortBy.ExpirationDateDescending => bulletins.OrderByDescending(bulletin => bulletin.ExpirationDate),
            null => bulletins,
            _ => throw new ArgumentOutOfRangeException(nameof(sortBy), sortBy, "Unexpected enum value")
        };
    }
}
