using Board.Application.Models.Search;
using Board.Domain;
using MediatR;

namespace Board.Application.Models.Users.Queries;

public sealed record SearchUserQuery(Page Page, string? Query, bool? IsAdmin) 
    : BaseSearchRequest(Page, Query), IRequest<SearchResult<User>>;
