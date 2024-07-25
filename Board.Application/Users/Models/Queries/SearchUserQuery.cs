using Board.Application.Models;
using Board.Domain;
using MediatR;

namespace Board.Application.Users.Models.Queries;

public sealed record SearchUserQuery(Page Page, string? Query, bool? IsAdmin, UserSortBy? SortBy) 
    : BaseSearchRequest(Page, Query), IRequest<SearchResult<User>>;
