﻿using Board.Application.Exceptions;
using Board.Application.Models.Search;
using Board.Application.Models.Users.Queries;
using Board.Application.Repositories;
using Board.Domain;
using Board.Infrastructure.Context;
using Board.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dbContext;

    public UserRepository(DataContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
    }

    public async Task Create(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _dbContext.Set<User>().AddAsync(user, cancellationToken);
    }

    public async Task<User> GetById(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<User>().SingleOrDefaultAsync(user => user.Id == userId, cancellationToken)
               ?? throw NotFoundException.CreateByUserId(userId);
    }

    public void Delete(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        _dbContext.Set<User>().Remove(user);
    }

    public async Task<SearchResult<User>> Search(SearchUserQuery query, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.Set<User>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Query))
        {
            queryable = queryable.Where(user => EF.Functions.ILike(user.Name, $"%{query.Query}%"));
        }

        if (query.IsAdmin is not null)
        {
            queryable = queryable.Where(user => user.IsAdmin == query.IsAdmin);
        }

        return await queryable.GetPagedAsync(query.Page, cancellationToken);
    }
}
