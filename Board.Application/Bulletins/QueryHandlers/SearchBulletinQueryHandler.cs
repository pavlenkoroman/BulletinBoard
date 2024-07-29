using Board.Application.Bulletins.Models.Queries;
using Board.Application.Models;
using Board.Application.Repositories;
using Board.Domain;
using MediatR;

namespace Board.Application.Bulletins.QueryHandlers;

public sealed class SearchBulletinQueryHandler : IRequestHandler<SearchBulletinQuery, SearchResult<Bulletin>>
{
    private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

    public SearchBulletinQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
    {
        ArgumentNullException.ThrowIfNull(tenantRepositoryFactory);

        _tenantRepositoryFactory = tenantRepositoryFactory;
    }

    public async Task<SearchResult<Bulletin>> Handle(SearchBulletinQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var tenant = _tenantRepositoryFactory.GetTenant();

        return await tenant.Bulletins.Search(request, cancellationToken);
    }
}
