using Board.Application.Models;
using Board.Application.Repositories;
using Board.Application.Users.Models.Queries;
using Board.Domain;
using MediatR;

namespace Board.Application.Users.QueryHandlers;

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, SearchResult<User>>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public SearchUserQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<SearchResult<User>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.Users.Search(request, cancellationToken);
    }
}
